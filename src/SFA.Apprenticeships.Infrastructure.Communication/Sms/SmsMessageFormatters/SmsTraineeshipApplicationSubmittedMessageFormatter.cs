namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Configuration;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class SmsTraineeshipApplicationSubmittedMessageFormatter : SmsMessageFormatter
    {
        public SmsTraineeshipApplicationSubmittedMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.TraineeshipApplicationSubmitted").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var commTokens = communicationTokens as IList<CommunicationToken> ?? communicationTokens.ToList();
            var vacancyTitle = commTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyTitle).Value;
            var employerName = commTokens.First(ct => ct.Key == CommunicationTokens.ApplicationVacancyEmployerName).Value;

            return string.Format(Message, vacancyTitle, employerName);
        }
    }
}