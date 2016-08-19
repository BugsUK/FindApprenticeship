namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class SmsSendEmailReminderMessageFormatter : SmsMessageFormatter
    {
        public SmsSendEmailReminderMessageFormatter(IConfigurationService configurationService) : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendEmailReminder").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.UserEmailAddress).Value);
        }
    }
}