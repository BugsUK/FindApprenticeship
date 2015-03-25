﻿namespace SFA.Apprenticeships.Infrastructure.Communication.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Configuration;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Newtonsoft.Json;

    public class SmsApprenticeshipApplicationExpiringDraftsSummaryMessageFormatter : SmsMessageFormatter
    {
        public SmsApprenticeshipApplicationExpiringDraftsSummaryMessageFormatter(IConfigurationService configurationService)
            : base(configurationService)
        {
            Message = GetTemplateConfiguration("MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary").Message;
        }

        public override string GetMessage(IEnumerable<CommunicationToken> communicationTokens)
        {
            var json = communicationTokens.First(t => t.Key == CommunicationTokens.ExpiringDrafts).Value;
            var expiringDrafts = JsonConvert.DeserializeObject<List<ExpiringApprenticeshipApplicationDraft>>(json);

            return string.Format(Message, expiringDrafts.Count);
        }
    }
}
