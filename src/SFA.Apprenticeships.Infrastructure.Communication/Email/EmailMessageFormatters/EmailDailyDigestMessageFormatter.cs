namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Application.Interfaces.Communications;
    using Configuration;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Newtonsoft.Json;
    using SendGrid;

    public class EmailDailyDigestMessageFormatter : EmailMessageFormatter
    {
        public const string OneSavedApplicationAboutToExpire = "<p>You've saved an application for an apprenticeship that is due to expire soon.</p>";
        public const string MoreThanOneSaveApplicationAboutToExpire = "<p>You've saved applications for apprenticeships that are due to close soon.</p>";

        private readonly string _siteDomainName;

        public EmailDailyDigestMessageFormatter(IConfigurationService configurationService)
        {
            _siteDomainName = configurationService.Get<CommunicationConfiguration>().CandidateSiteDomainName;
        }

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            PopulateCandidateName(request, message);
            PopulateApplicationStatusAlerts(request, message);
            PopulateExpiringDrafts(request, message);
            PopulateSiteDomainName(message);
            PopulateUnsubscribe(request, message);
        }

        #region Helpers

        private static void PopulateCandidateName(EmailRequest request, ISendGrid message)
        {
            var candidateFirstNameToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateFirstName);

            var substitutionText = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateFirstName).Value;

            AddSubstitutionTo(message, candidateFirstNameToken, substitutionText);
        }

        private void PopulateApplicationStatusAlerts(EmailRequest request, ISendGrid message)
        {
            var sendgridToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ApplicationStatusAlerts);

            var alertsJson = request.Tokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var alerts = JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(alertsJson) ?? new List<ApplicationStatusAlert>();

            var substitutionText = GetApplicationStatusAlertsInfoSubstitution(alerts);

            AddSubstitutionTo(message, sendgridToken, substitutionText);
        }

        protected string GetApplicationStatusAlertsInfoSubstitution(IList<ApplicationStatusAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder("<p>There has been an update on the following applications:</p>");

            if (alerts.Any(a => a.Status == ApplicationStatuses.Successful))
            {
                stringBuilder.AppendFormat("<b><a href=\"https://{0}/myapplications#dashSuccessful\">Successful applications</a></b>", _siteDomainName);
                stringBuilder.AppendLine();
                var successfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Successful).Select(d => string.Format("<li>{0} with {1}</li>", d.Title, d.EmployerName));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", successfulLineItems)));
            }

            if (alerts.Any(a => a.Status == ApplicationStatuses.Unsuccessful))
            {
                stringBuilder.AppendFormat("<b><a href=\"https://{0}/myapplications#dashUnsuccessful\">Unsuccessful applications</a></b>", _siteDomainName);
                stringBuilder.AppendLine();

                var unsuccessfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Unsuccessful).Select(
                    d =>
                        !string.IsNullOrWhiteSpace(d.UnsuccessfulReason)
                            ? $"<li>{d.Title} with {d.EmployerName}<br/><b>Reason: </b>{d.UnsuccessfulReason}</li>"
                            : $"<li>{d.Title} with {d.EmployerName}</li>");

                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join(string.Empty, unsuccessfulLineItems)));
                stringBuilder.Append("<p>If your application's unsuccessful ask your college or training provider for feedback.</p>");
                stringBuilder.AppendFormat("<p>For advice on writing better applications visit the <a href=\"https://{0}/nextsteps\">next steps page</a>.</p>", _siteDomainName);
            }

            return stringBuilder.ToString();
        }

        private void PopulateExpiringDrafts(EmailRequest request, ISendGrid message)
        {
            var stringBuilder = new StringBuilder();

            var draftsJson = request.Tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;
            var drafts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(draftsJson) ?? new List<ExpiringApprenticeshipApplicationDraft>();

            stringBuilder.Append(GetExpiringDraftsItemCountData(drafts));

            PopulateVacanciesDataSubstitution(drafts, stringBuilder);

            var sendgridToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ExpiringDrafts);
            
            var substitutionText = stringBuilder.ToString();

            AddSubstitutionTo(message, sendgridToken, substitutionText);
        }

        private string GetExpiringDraftsItemCountData(List<ExpiringApprenticeshipApplicationDraft> drafts)
        {
            var itemCount = drafts.Count;

            var substitutionText = string.Empty;
            if (itemCount > 0)
            {
                var itemCountSubstitution = itemCount == 1 ? OneSavedApplicationAboutToExpire : MoreThanOneSaveApplicationAboutToExpire;
                substitutionText = string.Format("<p><b><a href=\"https://{0}/myapplications#dashDrafts\">Saved applications due to expire</a></b></p>{1}", _siteDomainName, itemCountSubstitution);
            }

            return substitutionText;
        }

        private void PopulateVacanciesDataSubstitution(List<ExpiringApprenticeshipApplicationDraft> drafts, StringBuilder stringBuilder)
        {
            if (drafts == null || drafts.Count == 0) return;

            stringBuilder.Append("<ul>");

            foreach (var draft in drafts)
            {
                var draftListElement = string.Format("<li><a href=\"https://{4}/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>", draft.VacancyId, draft.Title, draft.EmployerName, draft.ClosingDate.ToLongDateString(), _siteDomainName);
                stringBuilder.Append(draftListElement);
            }

            stringBuilder.Append("</ul>");
        }

        private void PopulateSiteDomainName(ISendGrid message)
        {
            var token = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateSiteDomainName);

            AddSubstitutionTo(message, token, _siteDomainName);
        }

        private void PopulateUnsubscribe(EmailRequest request, ISendGrid message)
        {
            {
                var token = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateSubscriberId);
                var value = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateSubscriberId).Value;

                AddSubstitutionTo(message, token, value);
            }

            {
                var token = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateSubscriptionType);
                var value = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateSubscriptionType).Value;

                AddSubstitutionTo(message, token, value);
            }
        }

        private static void AddSubstitutionTo(ISendGrid message, string sendgridtoken, string substitutionText)
        {
            message.AddSubstitution(sendgridtoken, new List<string> {substitutionText});
        }

        #endregion
    }
}