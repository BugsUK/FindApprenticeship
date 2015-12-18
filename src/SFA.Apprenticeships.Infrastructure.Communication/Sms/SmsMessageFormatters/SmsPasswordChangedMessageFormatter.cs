namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Configuration;
    using SFA.Infrastructure.Interfaces;

    public class SmsPasswordChangedMessageFormatter : SmsMessageFormatter
    {
        public SmsPasswordChangedMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.PasswordChanged").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return Message;
        }
    }
}