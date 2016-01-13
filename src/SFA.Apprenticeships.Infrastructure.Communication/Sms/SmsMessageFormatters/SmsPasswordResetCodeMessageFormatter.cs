namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Configuration;
    using SFA.Infrastructure.Interfaces;

    public class SmsPasswordResetCodeMessageFormatter : SmsMessageFormatter
    {
        public SmsPasswordResetCodeMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.SendPasswordResetCode").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return string.Format(Message, communicationTokens.First(ct => ct.Key == CommunicationTokens.PasswordResetCode).Value);
        }
    }
}