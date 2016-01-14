namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Newtonsoft.Json;

    public class SmsApprenticeshipApplicationExpiringDraftMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationExpiringDraftMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationExpiringDraft").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var json = communicationTokens.First(t => t.Key == CommunicationTokens.ExpiringDraft).Value;
            var expiringDraft = JsonConvert.DeserializeObject<ExpiringApprenticeshipApplicationDraft>(json);

            // TODO: centralise date format handling.
            return string.Format(Message, expiringDraft.EmployerName, expiringDraft.ClosingDate.ToString("d MMM"));
        }
    }
}
