namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;

    public class SmsSendMobileVerificationCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsSendMobileVerificationCodeMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendMobileVerificationCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value);
        }
    }
}