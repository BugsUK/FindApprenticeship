namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies.Entities;
    using Application.Vacancies.Entities.SiteMap;
    using Domain.Interfaces.Messaging;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class VacancySummaryCompleteSubscriber : IServiceBusSubscriber<VacancySummaryUpdateComplete>
    {
        private readonly ILogService _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> _apprenticeshipVacancyIndexerService;
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> _traineeVacancyIndexerService;

        public VacancySummaryCompleteSubscriber(
            ILogService logger,
            IServiceBus serviceBus,
            IVacancyIndexerService<ApprenticeshipSummaryUpdate, Elastic.ApprenticeshipSummary> apprenticeshipVacancyIndexerService,
            IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> traineeVacancyIndexerService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _apprenticeshipVacancyIndexerService = apprenticeshipVacancyIndexerService;
            _traineeVacancyIndexerService = traineeVacancyIndexerService;
        }

        [ServiceBusTopicSubscription(TopicName = "VacancyIndexCreated")]
        public ServiceBusMessageStates Consume(VacancySummaryUpdateComplete updateComplete)
        {
            _logger.Debug("Received vacancy summary update completed message.");

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

                _serviceBus.PublishMessage(request);
            }
            else
            {
                _logger.Warn("One or more indexes was not correctly created (see previously logged errors), vacancy site map will not be created.");
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}