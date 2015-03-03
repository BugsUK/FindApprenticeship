namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Communication.Email.EmailMessageFormatters;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Moq;
    using SendGrid;

    public abstract class EmailDailyDigestMessageFormatterTestsBase
    {
        protected const string CandidateFirstNameTag = "-Candidate.FirstName-";
        protected const string ApplicationStatusAlertTag = "-Application.Status.Alert-";
        protected const string ExpiringDraftsTag = "-Expiring.Drafts-";

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

            stringBuilder.Append(expiringDrafts.Count == 1 ? EmailDailyDigestMessageFormatter.OneSavedApplicationAboutToExpire : EmailDailyDigestMessageFormatter.MoreThanOneSaveApplicationAboutToExpire);
            
            var lineItems = expiringDrafts.Select(d => string.Format("<li><a href=\"https://www.findapprenticeship.service.gov.uk/account/apprenticeshipvacancydetails/{0}\">{1} with {2}</a><br>Closing date: {3}</li>", d.VacancyId, d.Title, d.EmployerName, d.ClosingDate.ToLongDateString()));
            stringBuilder.Append(string.Format("<ul>{0}</ul>", string.Join("", lineItems)));

            return stringBuilder.ToString();
        }

        protected static string GetExpectedInfoSubstitution(IList<ApplicationStatusAlert> alerts)
        {
            if (alerts == null || alerts.Count == 0) return string.Empty;

            var stringBuilder = new StringBuilder("<p>You have received outcomes on some of your applications</p>");

            if (alerts.Any(a => a.Status == ApplicationStatuses.Successful))
            {
                stringBuilder.AppendLine("<b><a href=\"https://www.findapprenticeship.service.gov.uk/myapplications#dashSuccessful\">Applications made Successful</a></b>");
                var successfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Successful).Select(d => string.Format("<li>{0} with {1}</li>", d.Title, d.EmployerName));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", successfulLineItems)));
            }

            if (alerts.Any(a => a.Status == ApplicationStatuses.Unsuccessful))
            {
                stringBuilder.AppendLine("<b><a href=\"https://www.findapprenticeship.service.gov.uk/myapplications#dashUnsuccessful\">Applications made Unsuccessful</a></b>");
                var unsuccessfulLineItems = alerts.Where(a => a.Status == ApplicationStatuses.Unsuccessful).Select(d => string.Format("<li>{0} with {1}<br/><b>Reason: </b>{2}</li>", d.Title, d.EmployerName, d.UnsuccessfulReason));
                stringBuilder.AppendLine(string.Format("<ul>{0}</ul>", string.Join("", unsuccessfulLineItems)));
            }

            return stringBuilder.ToString();
        }
    }
}