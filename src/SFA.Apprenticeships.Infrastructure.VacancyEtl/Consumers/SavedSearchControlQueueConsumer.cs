namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;

    public class SavedSearchControlQueueConsumer: AzureControlQueueConsumer
    {
        private const string QueueName = "savedsearchscheduler";

        private readonly ILogService _logger;
        private readonly ISavedSearchProcessor _savedSearchProcessor;

        public SavedSearchControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService, ILogService logger, ISavedSearchProcessor savedSearchProcessor) : base(messageService, logger, "Saved Search Processor", QueueName)
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
                    MessageService.DeleteMessage(schedulerNotification.MessageId, schedulerNotification.PopReceipt, QueueName);
                    _logger.Info("Queued candidate saved searches and deleted message");
                }
            });
        }
    }
}
