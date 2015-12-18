namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Linq;
    using Application.Applications;
    using Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Monitor.Tasks;
    using Repository;

    public class CheckUnsetApprenticeshipApplicationLegacyId : IMonitorTask
    {
        private readonly IApprenticeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IServiceBus _serviceBus;
        private readonly ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;
        private readonly ILogService _logger;

        public CheckUnsetApprenticeshipApplicationLegacyId(
            IApprenticeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository,
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
            get { return "Check Unset Apprenticeship Application Legacy Id"; }
        }

        public void Run()
        {
            var applicationsToCheck = _applicationDiagnosticsRepository.GetSubmittedApplicationsWithUnsetLegacyId().ToList();

            foreach (var application in applicationsToCheck)
            {
                var applicationStatusSummaries = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(application.Candidate);
                var applicationDetail = application.ApprenticeshipApplicationDetail;
                var applicationStatusSummary = applicationStatusSummaries.SingleOrDefault(s => s.LegacyVacancyId == applicationDetail.Vacancy.Id);
                if (applicationStatusSummary == null)
                {
                    if (applicationDetail.Status != ApplicationStatuses.Submitting)
                    {
                        _applicationDiagnosticsRepository.UpdateApplicationStatus(applicationDetail, ApplicationStatuses.Submitting);
                    }

                    var message = new SubmitApprenticeshipApplicationRequest
                    {
                        ApplicationId = applicationDetail.EntityId
                    };

                    _serviceBus.PublishMessage(message);

                    _logger.Warn("Could not patch apprenticeship application id: {0} with legacy id as no matching application status summary was found. Re-queued instead", applicationDetail.EntityId);
                }
                else
                {
                    if (applicationDetail.Status != ApplicationStatuses.Submitted)
                    {
                        _applicationDiagnosticsRepository.UpdateApplicationStatus(applicationDetail, ApplicationStatuses.Submitted);
                    }
                    _applicationDiagnosticsRepository.UpdateLegacyApplicationId(applicationDetail, applicationStatusSummary.LegacyApplicationId);
                    _logger.Warn("Patching apprenticeship application id: {0} with legacy id: {1}", applicationDetail.EntityId, applicationStatusSummary.LegacyApplicationId);
                }
            }
        }
    }
}