namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Configuration;
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Newtonsoft.Json;

    public class SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var json = communicationTokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlerts).Value;
            var applicationStatusAlerts = JsonConvert.DeserializeObject<List<ApplicationStatusAlert>>(json);

            return string.Format(Message, applicationStatusAlerts.Count);
        }
    }
}
