﻿namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Azure.Common.Messaging;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class VacancyEtlControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IVacancySummaryProcessor _vacancySummaryProcessor;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _apprenticeshipVacancyIndexerService;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _traineeshipVacancyIndexerService;
        private readonly ILogService _logger;

        public VacancyEtlControlQueueConsumer(IProcessControlQueue<StorageQueueMessage> messageService,
            IVacancySummaryProcessor vacancySummaryProcessor,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipVacancyIndexerService,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> traineeshipVacancyIndexerService, 
            ILogService logger)
            : base(messageService, logger, "Vacancy ETL")
        {
            _vacancySummaryProcessor = vacancySummaryProcessor;
            _apprenticeshipVacancyIndexerService = apprenticeshipVacancyIndexerService;
            _traineeshipVacancyIndexerService = traineeshipVacancyIndexerService;
            _logger = logger;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    _logger.Debug("No scheduled message found on control queue");
                    return;
                }

                _logger.Info("Calling vacancy indexer service to create scheduled index");

                _apprenticeshipVacancyIndexerService.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);
                _traineeshipVacancyIndexerService.CreateScheduledIndex(latestScheduledMessage.ExpectedExecutionTime);

                _logger.Info("Calling vacancy summary processor to queue vacancy pages");
                
                _vacancySummaryProcessor.QueueVacancyPages(latestScheduledMessage);

                _logger.Info("Scheduled index created and vacancy pages queued");
            });
        }
    }
}
