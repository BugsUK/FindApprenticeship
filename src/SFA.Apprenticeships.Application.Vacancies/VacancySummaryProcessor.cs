namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Messaging;
    using Entities;

    using SFA.Apprenticeships.Application.Interfaces;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly ILogService _logger;

        private readonly IServiceBus _serviceBus;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IMapper _mapper;
        private readonly IJobControlQueue<StorageQueueMessage> _jobControlQueue;
        private readonly IApprenticeshipSummaryUpdateProcessor _apprenticeshipSummaryUpdateProcessor;
        private readonly ITraineeshipsSummaryUpdateProcessor _traineeshipsSummaryUpdateProcessor;

        public VacancySummaryProcessor(
            IServiceBus serviceBus,
            IVacancyIndexDataProvider vacancyIndexDataProvider,
            IMapper mapper,
            IJobControlQueue<StorageQueueMessage> jobControlQueue,
            IApprenticeshipSummaryUpdateProcessor apprenticeshipSummaryUpdateProcessor,
            ITraineeshipsSummaryUpdateProcessor traineeshipsSummaryUpdateProcessor,
            ILogService logger)
        {
            _serviceBus = serviceBus;
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _mapper = mapper;
            _jobControlQueue = jobControlQueue;
            _apprenticeshipSummaryUpdateProcessor = apprenticeshipSummaryUpdateProcessor;
            _traineeshipsSummaryUpdateProcessor = traineeshipsSummaryUpdateProcessor;
            _logger = logger;
        }

        public void ProcessVacancyPages(StorageQueueMessage scheduledQueueMessage)
        {
            _logger.Debug("Retrieving vacancy summary page count");

            var vacancyPageCount = _vacancyIndexDataProvider.GetVacancyPageCount();

            _logger.Info("Retrieved vacancy summary page count of {0}", vacancyPageCount);

            if (vacancyPageCount == 0)
            {
                _logger.Warn("Expected vacancy page count to be greater than zero. Indexes will not be created successfully");
                _jobControlQueue.DeleteMessage(ScheduledJobQueues.VacancyEtl, scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);
                return;
            }

            var vacancySummaries = BuildVacancySummaryPages(scheduledQueueMessage.ExpectedExecutionTime, vacancyPageCount).ToList();

            // Only delete from queue once we have all vacancies from the service without error.
            _jobControlQueue.DeleteMessage(ScheduledJobQueues.VacancyEtl, scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            //Process pages
            Parallel.ForEach(vacancySummaries, new ParallelOptions { MaxDegreeOfParallelism = 1 }, ProcessVacancySummaryPage);

            var lastVacancySummaryPage = vacancySummaries.Last();

            _logger.Info("Vacancy ETL Queue completed: {0} vacancy summary pages processed ", lastVacancySummaryPage.TotalPages);

            _logger.Info("Publishing VacancySummaryUpdateComplete message to queue");

            var vsuc = new VacancySummaryUpdateComplete
            {
                ScheduledRefreshDateTime = lastVacancySummaryPage.ScheduledRefreshDateTime
            };

            _serviceBus.PublishMessage(vsuc);

            _logger.Info("Published VacancySummaryUpdateComplete message published to queue");
        }

        private void ProcessVacancySummaryPage(VacancySummaryPage vacancySummaryPage)
        {
            _logger.Info("Retrieving vacancy search page number: {0}/{1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);

            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.PageNumber);
            var apprenticeshipsExtended = _mapper.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(vacancies.ApprenticeshipSummaries).ToList();
            var traineeshipsExtended = _mapper.Map<IEnumerable<TraineeshipSummary>, IEnumerable<TraineeshipSummaryUpdate>>(vacancies.TraineeshipSummaries).ToList();

            _logger.Info("Retrieved vacancy search page number: {0}/{1} with {2} apprenticeships and {3} traineeships",
                vacancySummaryPage.PageNumber,
                vacancySummaryPage.TotalPages,
                apprenticeshipsExtended.Count,
                traineeshipsExtended.Count);

            Parallel.ForEach(
                apprenticeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                apprenticeshipExtended =>
                {
                    apprenticeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _apprenticeshipSummaryUpdateProcessor.Process(apprenticeshipExtended);
                });

            _logger.Info("Processed {0} apprenticeships", apprenticeshipsExtended.Count);

            Parallel.ForEach(
                traineeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                traineeshipExtended =>
                {
                    traineeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _traineeshipsSummaryUpdateProcessor.Process(traineeshipExtended);
                });

            _logger.Info("Processed {0} traineeships", traineeshipsExtended.Count);

            _logger.Info("Processed vacancy search page number: {0}/{1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);
        }

        #region Helpers
        private static IEnumerable<VacancySummaryPage> BuildVacancySummaryPages(DateTime scheduledRefreshDateTime, int count)
        {
            var vacancySumaries = new List<VacancySummaryPage>(count);

            for (var i = 1; i <= count; i++)
            {
                var vacancySummaryPage = new VacancySummaryPage
                {
                    PageNumber = i,
                    TotalPages = count,
                    ScheduledRefreshDateTime = scheduledRefreshDateTime
                };

                vacancySumaries.Add(vacancySummaryPage);
            }

            return vacancySumaries;
        }

        #endregion
    }
}
