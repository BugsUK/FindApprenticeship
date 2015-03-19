namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;

    public class SmsApprenticeshipApplicationUnsuccessfulMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationUnsuccessfulMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations) 
            : base(templateConfigurations)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationUnsuccessful").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var json = communicationTokens.First(t => t.Key == CommunicationTokens.ApplicationStatusAlert).Value;
            var applicationStatusAlert = JsonConvert.DeserializeObject<ApplicationStatusAlert>(json);

            return string.Format(Message, applicationStatusAlert.EmployerName);
        }
    }
}
