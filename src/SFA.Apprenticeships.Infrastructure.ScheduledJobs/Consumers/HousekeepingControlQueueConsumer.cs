namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using Application.Applications.Housekeeping;
    using Application.Candidates;
    using Application.Communications.Housekeeping;
    using Application.Interfaces.Logging;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class HousekeepingControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ICandidateProcessor _candidateProcessor;
        private readonly IRootApplicationHousekeeper _rootApplicationHousekeeper;
        private readonly IRootCommunicationHousekeeper _rootCommunicationHousekeeper;

        public HousekeepingControlQueueConsumer(
            ILogService logger,
            IJobControlQueue<StorageQueueMessage> messageService,
            ICandidateProcessor candidateProcessor,
            IRootApplicationHousekeeper rootApplicationHousekeeper,
            IRootCommunicationHousekeeper rootCommunicationHousekeeper)
            : base(messageService, logger, "Housekeeping", ScheduledJobQueues.Housekeeping)
        {
            _candidateProcessor = candidateProcessor;
            _rootApplicationHousekeeper = rootApplicationHousekeeper;
            _rootCommunicationHousekeeper = rootCommunicationHousekeeper;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();

                if (schedulerNotification == null) return;

                _candidateProcessor.QueueCandidates();
                
                // TODO: US794: enable housekeeping.
                _rootApplicationHousekeeper.QueueHousekeepingRequests();
                _rootCommunicationHousekeeper.QueueHousekeepingRequests();

                MessageService.DeleteMessage(QueueName, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
            });
        }
    }
}