namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Newtonsoft.Json;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SmsApprenticeshipApplicationUnsuccessfulMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationUnsuccessfulMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
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
