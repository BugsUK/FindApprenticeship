namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Email.EmailMessageFormatters
{
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Helpers;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
    using Moq;
    using SendGrid;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class EmailDailyDigestMessageFormatterTestsBase
    {
        protected const string CandidateFirstNameTag = "-Candidate.FirstName-";
        protected const string ApplicationStatusAlertTag = "-Application.Status.Alert-";
        protected const string ExpiringDraftsTag = "-Expiring.Drafts-";

        protected const string SiteDomainName = "test.findapprenticeship.service.gov.uk";

        protected static Mock<ISendGrid> GetSendGridMessage(out List<SendGridMessageSubstitution> sendGridMessageSubstitutions)
        {
            var sendGridMessage = new Mock<ISendGrid>();
            var substitutions = new List<SendGridMessageSubstitution>();
            sendGridMessage.Setup(m => m.AddSubstitution(It.IsAny<string>(), It.IsAny<List<string>>()))
                .Callback<string, List<string>>(
                    (rt, sv) => substitutions.Add(new SendGridMessageSubstitution(rt, sv)));

            sendGridMessageSubstitutions = substitutions;
            return sendGridMessage;
        }

        protected static string GetExpectedInfoSubstitution(IList<ExpiringApprenticeshipApplicationDraft> expiringDrafts)
        {
            if (expiringDrafts == null || expiringDrafts.Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<p><b><a href=\"https://" + SiteDomainName + "/myapplications#dashDrafts\">Saved applications due to expire</a></b></p>");
            stringBuilder.Append(expiringDrafts.Count == 1 ? EmailDailyDigestMessageFormatter.OneSavedApplicationAboutToExpire : EmailDailyDigestMessageFormatter.MoreThanOneSaveApplicationAboutToExpire);

            var lineItems = expiringDrafts.Select(d => string.Format("<li><a href=\"https://" + SiteDomainName + "/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>", d.VacancyId, d.Title, d.EmployerName, d.ClosingDate.ToLongDateString()));
            stringBuilder.Append(string.Format("<ul>{0}</ul>", string.Join("", lineItems)));

            return stringBuilder.ToString();
        }

        protected static string GetExpectedInfoSubstitution(IList<ApplicationStatusAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder("<p>There has been an update on the following applications:</p>");

            if (alerts.Any(a => a.Status == ApplicationStatuses.Successful))
            {
                stringBuilder.AppendLine("<b><a href=\"https://" + SiteDomainName + "/myapplications#dashSuccessful\">Successful applications</a></b>");
                var successfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Successful).Select(d => string.Format("<li>{0} with {1}</li>", d.Title, d.EmployerName));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", successfulLineItems)));
            }

            if (alerts.Any(a => a.Status == ApplicationStatuses.Unsuccessful))
            {
                if (alerts.Any(a => a.Status == ApplicationStatuses.Unsuccessful))
                {
                    stringBuilder.AppendFormat($"<b><a href=\"https://{SiteDomainName}/myapplications#dashUnsuccessful\">Unsuccessful applications</a></b>");
                    stringBuilder.AppendLine();

                    var unsuccessfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Unsuccessful).Select(
                        d =>
                            !string.IsNullOrWhiteSpace(d.UnsuccessfulReason)
                                ? $"<li>{d.Title} with {d.EmployerName}<br/>" +
                                  $"<a href=\"https://{SiteDomainName}/apprenticeship/candidateapplicationfeedback/{d.VacancyId}\">Read feedback</a></li>"
                                : $"<li>{d.Title} with {d.EmployerName}</li>");

                    stringBuilder.AppendLine($"<ul>{string.Join(string.Empty, unsuccessfulLineItems)}</ul>");
                    stringBuilder.Append("<p>If your application's unsuccessful ask your college or training provider for feedback.</p>");
                    stringBuilder.AppendFormat($"<p>For advice on writing better applications visit the <a href=\"https://{SiteDomainName}/nextsteps\">next steps page</a>.</p>");
                }
            }

            return stringBuilder.ToString();
        }
    }
}