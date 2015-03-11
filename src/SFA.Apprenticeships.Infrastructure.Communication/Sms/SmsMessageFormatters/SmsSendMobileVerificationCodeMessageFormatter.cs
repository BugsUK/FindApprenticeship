namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;

    public class SmsSendMobileVerificationCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsSendMobileVerificationCodeMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations)
            : base(templateConfigurations)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendMobileVerificationCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value);
        }
    }
}