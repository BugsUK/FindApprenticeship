namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;
    using Configuration;
    using Domain.Interfaces.Configuration;

    public class SmsSavedSearchAlertMessageFormatter : SmsMessageFormatter
    {
        public SmsSavedSearchAlertMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.SavedSearchAlert").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            return Message;
        }
    }
}