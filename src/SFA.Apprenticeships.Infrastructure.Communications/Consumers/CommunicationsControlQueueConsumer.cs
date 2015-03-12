namespace SFA.Apprenticeships.Infrastructure.Communications.Consumers
{
    using System.Threading.Tasks;
    using Application.Communications;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class CommunicationsControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICommunicationProcessor _communicationProcessor;

        public CommunicationsControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            ICommunicationProcessor communicationProcessor, ILogService logger)
            : base(messageService, logger, "Communications")
        {
            _communicationProcessor = communicationProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();

                if (schedulerNotification == null) return;

                _communicationProcessor.SendDailyDigests(schedulerNotification.ClientRequestId);
                _communicationProcessor.SendSavedSearchAlerts(schedulerNotification.ClientRequestId);

                MessageService.DeleteMessage(schedulerNotification.MessageId, schedulerNotification.PopReceipt);
            });
        }
    }
}
