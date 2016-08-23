namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.Consumers
{
    using System.Threading.Tasks;
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class SavedSearchControlQueueConsumer: AzureControlQueueConsumer
    {
        private readonly ILogService _logger;
        private readonly ISavedSearchProcessor _savedSearchProcessor;

        public SavedSearchControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, ILogService logger, ISavedSearchProcessor savedSearchProcessor)
            : base(messageService, logger, "Saved Search Processor", ScheduledJobQueues.SavedSearch)
        {
            _logger = logger;
            _savedSearchProcessor = savedSearchProcessor;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var schedulerNotification = GetLatestQueueMessage();
                if (schedulerNotification != null)
                {
                    _logger.Info("Calling saved search processor to queue candidate saved searches");
                    _savedSearchProcessor.QueueCandidateSavedSearches();
                    MessageService.DeleteMessage(ScheduledJobQueues.SavedSearch, schedulerNotification.MessageId, schedulerNotification.PopReceipt);
                    _logger.Info("Queued candidate saved searches and deleted message");
                }
            });
        }
    }
}
