namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> _apprenticeshipVacancyIndexerService;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _trainseeshipVacancyIndexerService;
        private readonly ILogService _logger;

        public VacancySummaryCompleteConsumerAsync(
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary> apprenticeshipVacancyIndexerService,
            IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> trainseeshipVacancyIndexerService, ILogService logger)
        {
            _apprenticeshipVacancyIndexerService = apprenticeshipVacancyIndexerService;
            _trainseeshipVacancyIndexerService = trainseeshipVacancyIndexerService;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            _logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                if (_apprenticeshipVacancyIndexerService.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _logger.Info("Swapping apprenticeship index alias after vacancy summary update completed");
                    _apprenticeshipVacancyIndexerService.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index apprenticeship swapped after vacancy summary update completed");
                }
                else
                {
                    _logger.Error("The new apprenticeship index is not correctly created. Aborting swap.");
                }

                if (_trainseeshipVacancyIndexerService.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime))
                {
                    _logger.Info("Swapping traineeship index alias after vacancy summary update completed");
                    _trainseeshipVacancyIndexerService.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index traineeship swapped after vacancy summary update completed");
                }
                else
                {
                    _logger.Error("The new traineeship index is not correctly created. Aborting swap.");
                }
            });
        }
    }
}
