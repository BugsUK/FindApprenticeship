namespace SFA.Apprenticeships.Application.Communications
{
    using System;
    using Strategies;

    public class CommunicationProcessor : ICommunicationProcessor
    {
        private readonly ISendDailyDigestsStrategy _sendDailyDigestsStrategy;
        private readonly ISendSavedSearchAlertsStrategy _sendSavedSearchAlertsStrategy;

        public CommunicationProcessor(
            ISendDailyDigestsStrategy sendDailyDigestsStrategy,
            ISendSavedSearchAlertsStrategy sendSavedSearchAlertsStrategy)
        {
            _sendDailyDigestsStrategy = sendDailyDigestsStrategy;
            _sendSavedSearchAlertsStrategy = sendSavedSearchAlertsStrategy;
        }

        public void SendDailyDigests(Guid batchId)
        {
            _sendDailyDigestsStrategy.SendDailyDigests(batchId);
        }

        public void SendSavedSearchAlerts(Guid batchId)
        {
            _sendSavedSearchAlertsStrategy.SendSavedSearchAlerts(batchId);
        }
    }
}