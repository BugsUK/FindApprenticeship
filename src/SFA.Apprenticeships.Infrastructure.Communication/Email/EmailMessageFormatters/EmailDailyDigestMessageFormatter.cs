namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Application.Interfaces.Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;
    using SendGrid;

    public class EmailDailyDigestMessageFormatter : EmailMessageFormatter
    {
        public const string OneSavedApplicationAboutToExpire = "<p>You've saved an application for an apprenticeship that is due to expire soon.</p>";
        public const string MoreThanOneSaveApplicationAboutToExpire = "<p>You've saved applications for apprenticeships that are due to close soon.</p>";

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            PopulateCandidateName(request, message);

            PopulateApplicationStatusAlerts(request, message);

            PopulateExpiringDrafts(request, message);
        }

        private static void PopulateCandidateName(EmailRequest request, ISendGrid message)
        {
            var candidateFirstNameToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateFirstName);

            var substitutionText = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateFirstName).Value;

            AddSubstitutionTo(message, candidateFirstNameToken, substitutionText);
        }

        private static void PopulateApplicationStatusAlerts(EmailRequest request, ISendGrid message)
        {
            var sendgridToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.ApplicationStatusAlerts);

            var alertsJson = request.Tokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var alerts = JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(alertsJson) ?? new List<ApplicationStatusAlert>();

            var substitutionText = GetApplicationStatusAlertsInfoSubstitution(alerts);

            AddSubstitutionTo(message, sendgridToken, substitutionText);
        }

        protected static string GetApplicationStatusAlertsInfoSubstitution(IList<ApplicationStatusAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder("<p>There has been a decision on the following applications:</p>");

            if (alerts.Any(a => a.Status == ApplicationStatuses.Successful))
            {
                stringBuilder.AppendLine("<b><a href=\"https://www.findapprenticeship.service.gov.uk/myapplications#dashSuccessful\">Successful applications</a></b>");
                var successfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Successful).Select(d => string.Format("<li>{0} with {1}</li>", d.Title, d.EmployerName));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", successfulLineItems)));
            }

            if (alerts.Any(a => a.Status == ApplicationStatuses.Unsuccessful))
            {
                stringBuilder.AppendLine("<b><a href=\"https://www.findapprenticeship.service.gov.uk/myapplications#dashUnsuccessful\">Unsuccessful applications</a></b>");
                var unsuccessfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Unsuccessful).Select(d => string.Format("<li>{0} with {1}<br/><b>Reason: </b>{2}</li>", d.Title, d.EmployerName, d.UnsuccessfulReason));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", unsuccessfulLineItems)));
                stringBuilder.AppendLine("<p>For unsuccessful applications please contact the training provider for further information.</p><p>For careers advice and support contact the <a href=\"https://nationalcareersservice.direct.gov.uk/pages/home.aspx\">National Careers Service</a></p>");
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

        private static string GetExpiringDraftsItemCountData(List<ExpiringApprenticeshipApplicationDraft> drafts)
        {
            var itemCount = drafts.Count;

            var substitutionText = string.Empty;
            if (itemCount > 0)
            {
                substitutionText = "<p>Saved applications due to expire</p>" + (itemCount == 1 ? OneSavedApplicationAboutToExpire : MoreThanOneSaveApplicationAboutToExpire);
            }

            return substitutionText;
        }

        private static void PopulateVacanciesDataSubstitution(List<ExpiringApprenticeshipApplicationDraft> drafts, StringBuilder stringBuilder)
        {
            if (drafts == null || drafts.Count == 0) return;

            stringBuilder.Append("<ul>");

            foreach (var draft in drafts)
            {
                var draftListElement = string.Format("<li><a href=\"https://www.findapprenticeship.service.gov.uk/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>", draft.VacancyId, draft.Title, draft.EmployerName, draft.ClosingDate.ToLongDateString());
                stringBuilder.Append(draftListElement);
            }

            stringBuilder.Append("</ul>");
        }

        private static void AddSubstitutionTo(ISendGrid message, string sendgridtoken, string substitutionText)
        {
            message.AddSubstitution(sendgridtoken, new List<string> {substitutionText});
        }
    }
}