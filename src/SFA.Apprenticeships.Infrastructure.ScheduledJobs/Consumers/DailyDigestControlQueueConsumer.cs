namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using Application.Communications;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class DailyDigestControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICommunicationProcessor _communicationProcessor;

        public DailyDigestControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService,
            ICommunicationProcessor communicationProcessor, ILogService logger)
            : base(messageService, logger, "Communications", ScheduledJobQueues.DailyDigest)
        {
            _communicationProcessor = communicationProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();

                if (schedulerNotification == null) return;

                // TODO: AG: US638: put in separate try / catch blocks, implement in separate classes.
                _communicationProcessor.SendDailyDigests(schedulerNotification.ClientRequestId);
                _communicationProcessor.SendSavedSearchAlerts(schedulerNotification.ClientRequestId);

                MessageService.DeleteMessage(ScheduledJobQueues.DailyDigest, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
            });
        }
    }
}
