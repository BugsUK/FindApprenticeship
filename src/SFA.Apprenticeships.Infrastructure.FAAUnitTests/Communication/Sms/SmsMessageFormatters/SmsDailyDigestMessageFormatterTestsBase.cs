namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Domain.Entities.Communication;

    public abstract class SmsDailyDigestMessageFormatterTestsBase
    {
        protected const string MessageTemplate = "There has been a decision on the following applications:\n{0}\n|You have {0} saved applications for apprenticeships that are due to close soon:\n{1}\nYou can check the status of your applications in the ‘My applications’ section of your account.";
        protected const int MaxAlertCount = 3;
        protected const int MaxDraftCount = 3;

        protected static string GetExpectedMessage(List<ExpiringApprenticeshipApplicationDraft> expiringDrafts, IEnumerable<ApplicationStatusAlert> alerts, out int draftCount, out int draftLineCount, out int alertCount, out int alertLineCount)
        {
            var messageTemplateSections = MessageTemplate.Split('|');

            var alertsMessage = string.Empty;
            var alertsLineItems = new List<string>();
            alertCount = 0;
            alertLineCount = 0;
            if (alerts != null)
            {
                foreach (var alert in alerts)
                {
                    alertCount++;
                    if (alertCount <= MaxAlertCount)
                    {
                        alertLineCount++;
                        var lineItem = string.Format("{0}) With {1}", alertCount, alert.EmployerName);
                        alertsLineItems.Add(lineItem);
                    }
                }
                if (alertCount > 0)
                {
                    alertsMessage = string.Format(messageTemplateSections[0], string.Join("\n", alertsLineItems));
                }
            }

            var expiringDraftsMessage = string.Empty;
            var expiringDraftsLineItems = new List<string>();
            draftCount = 0;
            draftLineCount = 0;
            if (expiringDrafts != null)
            {
                foreach (var expiringDraft in expiringDrafts)
                {
                    draftCount++;
                    if (draftCount <= MaxDraftCount)
                    {
                        draftLineCount++;
                        var lineItem = string.Format("{0}) With {1}, closing date {2}", draftCount, expiringDraft.EmployerName, expiringDraft.ClosingDate.ToLongDateString());
                        expiringDraftsLineItems.Add(lineItem);
                    }
                }
                if (draftCount > 0)
                {
                    expiringDraftsMessage = string.Format(messageTemplateSections[1], draftCount, string.Join("\n", expiringDraftsLineItems));
                }
            }

            return alertsMessage + expiringDraftsMessage;
        }
    }
}