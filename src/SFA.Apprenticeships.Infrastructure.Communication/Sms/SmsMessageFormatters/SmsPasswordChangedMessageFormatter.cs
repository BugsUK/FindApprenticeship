namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public class SmsPasswordChangedMessageFormatter : SmsMessageFormatter
    {
        public SmsPasswordChangedMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations)
            : base(templateConfigurations)
        {
            Message = GetTemplateConfiguration("MessageTypes.PasswordChanged").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return Message;
        }
    }
}