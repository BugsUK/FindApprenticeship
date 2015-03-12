namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public class SmsSavedSearchAlertMessageFormatter : SmsMessageFormatter
    {
        public SmsSavedSearchAlertMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations)
            : base(templateConfigurations)
        {
            Message = GetTemplateConfiguration("MessageTypes.SavedSearchAlert").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return Message;
        }
    }
}