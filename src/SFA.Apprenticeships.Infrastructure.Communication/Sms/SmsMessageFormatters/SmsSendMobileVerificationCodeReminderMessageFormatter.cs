namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Configuration;

    public class SmsSendMobileVerificationCodeReminderMessageFormatter : SmsMessageFormatter
    {
        public SmsSendMobileVerificationCodeReminderMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendMobileVerificationCodeReminder").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value);
        }
    }
}