namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;

    public class SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationsUnsuccessfulSummaryMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations)
            : base(templateConfigurations)
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
