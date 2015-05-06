namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using Application.Candidates;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class HousekeepingControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICandidateProcessor _candidateProcessor;

        public HousekeepingControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, ICandidateProcessor candidateProcessor, ILogService logger)
            : base(messageService, logger, "Housekeeping", ScheduledJobQueues.Housekeeping)
        {
            _candidateProcessor = candidateProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();

                if (schedulerNotification == null) return;

                _candidateProcessor.QueueCandidates();

                MessageService.DeleteMessage(QueueName, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
            });
        }
    }
}