namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using Application.Application;
    using Application.Candidate;
    using Domain.Interfaces.Messaging;
    using Monitor.Tasks;
    using Repository;
    using SFA.Apprenticeships.Application.Interfaces;
    using System.Linq;

    public class CheckUnsetTraineeshipApplicationLegacyId : IMonitorTask
    {
        private readonly ITraineeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IServiceBus _serviceBus;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly ILogService _logger;

        public CheckUnsetTraineeshipApplicationLegacyId(
            ITraineeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository,
            IServiceBus serviceBus,
            ILegacyApplicationStatusesProvider legacyApplicationStatusesProvider,
            ILogService logger)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _serviceBus = serviceBus;
            _legacyApplicationStatusesProvider = legacyApplicationStatusesProvider;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Unset Traineeship Application Legacy Id"; }
        }

        public void Run()
        {
            var applicationsToCheck = _applicationDiagnosticsRepository.GetSubmittedApplicationsWithUnsetLegacyId().ToList();

            foreach (var application in applicationsToCheck)
            {
                var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(application.Candidate);
                var applicationDetail = application.TraineeshipApplicationDetail;
                var applicationStatusSummary = applicationStatusSummaries.SingleOrDefault(s => s.LegacyVacancyId == applicationDetail.Vacancy.Id);
                if (applicationStatusSummary == null)
                {
                    var message = new SubmitTraineeshipApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _serviceBus.PublishMessage(message);

                    _logger.Warn("Could not patch traineeship application id: {0} with legacy id as no matching application status summary was found. Re-queued instead", applicationDetail.EntityId);
                }
                else
                {
                    _applicationDiagnosticsRepository.UpdateLegacyApplicationId(applicationDetail, applicationStatusSummary.LegacyApplicationId);
                    _logger.Warn("Patching traineeship application id: {0} with legacy id: {1}", applicationDetail.EntityId, applicationStatusSummary.LegacyApplicationId);
                }
            }
        }
    }
}