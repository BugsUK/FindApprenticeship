namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Messaging;
    using Entities;
    using Interfaces.Logging;

    public class VacancySummaryProcessor : IVacancySummaryProcessor
    {
        private readonly ILogService _logger;

        private readonly IMessageBus _messageBus;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IMapper _mapper;
        private readonly IJobControlQueue<StorageQueueMessage> _jobControlQueue;

        public VacancySummaryProcessor(IMessageBus messageBus,
                                       IVacancyIndexDataProvider vacancyIndexDataProvider,
                                       IMapper mapper,
                                       IJobControlQueue<StorageQueueMessage> jobControlQueue, 
                                       ILogService logger)
        {
            _messageBus = messageBus;
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _mapper = mapper;
            _jobControlQueue = jobControlQueue;
            _logger = logger;
        }

        public void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage)
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

            foreach (var vacancySummaryPage in vacancySummaries)
            {
                _messageBus.PublishMessage(vacancySummaryPage);
            }

            // Only delete from queue once we have all vacancies from the service without error.
            _jobControlQueue.DeleteMessage(ScheduledJobQueues.VacancyEtl, scheduledQueueMessage.MessageId, scheduledQueueMessage.PopReceipt);

            _logger.Info("Queued {0} vacancy summary pages", vacancySummaries.Count());
        }
      
        public void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage)
        {
            _logger.Info("Retrieving vacancy search page number: {0}/{1}", vacancySummaryPage.PageNumber, vacancySummaryPage.TotalPages);

            var vacancies = _vacancyIndexDataProvider.GetVacancySummaries(vacancySummaryPage.PageNumber);
            var apprenticeshipsExtended = _mapper.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(vacancies.ApprenticeshipSummaries).ToList();
            var traineeshipsExtended = _mapper.Map<IEnumerable<TraineeshipSummary>, IEnumerable<TraineeshipSummaryUpdate>>(vacancies.TraineeshipSummaries).ToList();

            _logger.Info("Retrieved vacancy search page number: {0}/{1} with {2} apprenticeships and {3} traineeships", 
                vacancySummaryPage.PageNumber, 
                vacancySummaryPage.TotalPages, 
                apprenticeshipsExtended.Count(), 
                traineeshipsExtended.Count());

            Parallel.ForEach(
                apprenticeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                apprenticeshipExtended =>
                {
                    apprenticeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _messageBus.PublishMessage(apprenticeshipExtended);
                });

            Parallel.ForEach(
                traineeshipsExtended,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                traineeshipExtended =>
                {
                    traineeshipExtended.ScheduledRefreshDateTime = vacancySummaryPage.ScheduledRefreshDateTime;
                    _messageBus.PublishMessage(traineeshipExtended);
                });
        }

        public void QueueVacancyIfExpiring(ApprenticeshipSummary vacancySummary, int aboutToExpireThreshold)
        {
            try
            {
                if (vacancySummary.ClosingDate < DateTime.Now.AddHours(aboutToExpireThreshold))
                {
                    _logger.Debug("Queueing expiring vacancy");

                    var vacancyAboutToExpireMessage = new VacancyAboutToExpire { Id = vacancySummary.Id };
                    _messageBus.PublishMessage(vacancyAboutToExpireMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed queueing expiring vacancy {0}", ex, vacancySummary.Id);
            }
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
