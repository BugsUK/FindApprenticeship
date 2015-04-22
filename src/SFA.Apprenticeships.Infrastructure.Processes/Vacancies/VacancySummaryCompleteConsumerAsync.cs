namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies.Entities;
    using Application.Vacancies.Entities.SiteMap;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class VacancySummaryCompleteConsumerAsync : IConsumeAsync<VacancySummaryUpdateComplete>
    {
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> _apprenticeshipVacancyIndexerService;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> _traineeVacancyIndexerService;
        private readonly ILogService _logger;
        private readonly IMessageBus _messageBus;

        public VacancySummaryCompleteConsumerAsync(
            ILogService logger,
            IMessageBus messageBus,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> apprenticeshipVacancyIndexerService,
            IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> traineeVacancyIndexerService)
        {
            _logger = logger;
            _messageBus = messageBus;
            _apprenticeshipVacancyIndexerService = apprenticeshipVacancyIndexerService;
            _traineeVacancyIndexerService = traineeVacancyIndexerService;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryCompleteConsumerAsync")]
        public Task Consume(VacancySummaryUpdateComplete updateComplete)
        {
            _logger.Debug("Received vacancy summary update completed message.");

            return Task.Run(() =>
            {
                var isApprenticeshipIndexCorrectlyCreated = _apprenticeshipVacancyIndexerService.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime);
                var apprenticeshipVacancyIndexName = default(string);

                if (isApprenticeshipIndexCorrectlyCreated)
                {
                    _logger.Info("Swapping apprenticeship index alias after vacancy summary update completed");
                    apprenticeshipVacancyIndexName = _apprenticeshipVacancyIndexerService.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index apprenticeship swapped after vacancy summary update completed: '{0}'", apprenticeshipVacancyIndexName);
                }
                else
                {
                    _logger.Error("The new apprenticeship index is not correctly created. Aborting swap.");
                }

                var isTraineeshipIndexCorrectlyCreated = _traineeVacancyIndexerService.IsIndexCorrectlyCreated(updateComplete.ScheduledRefreshDateTime);
                var traineeshipVacancyIndexName = default(string);

                if (isTraineeshipIndexCorrectlyCreated)
                {
                    _logger.Info("Swapping traineeship index alias after vacancy summary update completed");
                    traineeshipVacancyIndexName = _traineeVacancyIndexerService.SwapIndex(updateComplete.ScheduledRefreshDateTime);
                    _logger.Info("Index traineeship swapped after vacancy summary update completed: '{0}'", traineeshipVacancyIndexName);
                }
                else
                {
                    _logger.Error("The new traineeship index is not correctly created. Aborting swap.");
                }

                if (isApprenticeshipIndexCorrectlyCreated && isTraineeshipIndexCorrectlyCreated)
                {
                    var request = new CreateVacancySiteMapRequest
                    {
                        ApprenticeshipVacancyIndexName = apprenticeshipVacancyIndexName,
                        TraineeshipVacancyIndexName = traineeshipVacancyIndexName
                    };

                    _messageBus.PublishMessage(request);
                }
                else
                {
                    _logger.Warn("One or more indexes was not correctly created (see previously logged errors), vacancy site map will not be created.");
                }
            });
        }
    }
}