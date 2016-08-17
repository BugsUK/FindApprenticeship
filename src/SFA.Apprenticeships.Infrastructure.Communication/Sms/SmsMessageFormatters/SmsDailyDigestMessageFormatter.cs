namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SmsDailyDigestMessageFormatter : SmsMessageFormatter
    {
        public const string TemplateName = "MessageTypes.DailyDigest";

        private const string AlertsSummaryFormat = "{0}) With {1}";
        private const string ExpiringDraftSummaryFormat = "{0}) With {1}, closing date {2}";
        private const int MaxAlertCount = 3;
        private const int MaxDraftCount = 3;

        public SmsDailyDigestMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration(TemplateName).Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var tokens = communicationTokens as IList<CommunicationToken> ?? communicationTokens.ToList();

            var expiringDraftsString = tokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;
            var expiringDrafts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(expiringDraftsString);

            var alertsJson = tokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var alerts = JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(alertsJson);

            return GetMessage(expiringDrafts, alerts);
        }

        private string GetMessage(IEnumerable<ExpiringApprenticeshipApplicationDraft> expiringDrafts, IEnumerable<ApplicationStatusAlert> alerts)
        {
            var messageTemplateSections = Message.Split('|');

            var alertsMessage = string.Empty;
            var alertsLineItems = new List<string>();
            var alertCount = 0;
            if (alerts != null)
            {
                foreach (var alert in alerts)
                {
                    alertCount++;
                    if (alertCount <= MaxAlertCount)
                    {
                        var lineItem = string.Format(AlertsSummaryFormat, alertCount, alert.EmployerName);
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
            var draftCount = 0;

            if (expiringDrafts != null)
            {
                foreach (var expiringDraft in expiringDrafts)
                {
                    draftCount++;
                    if (draftCount <= MaxDraftCount)
                    {
                        var lineItem = string.Format(ExpiringDraftSummaryFormat, draftCount, expiringDraft.EmployerName, expiringDraft.ClosingDate.ToLongDateString());
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