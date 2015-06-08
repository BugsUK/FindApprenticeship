namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates;
    using Candidates.Configuration;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Strategies;

    public class SubmittedApplicationHousekeeper : ISubmittedApplicationHousekeeper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IAuditApplicationDetailStrategy _auditApplicationDetailStrategy;
        private readonly IHardDeleteApplicationStrategy _hardDeleteApplicationStrategy;

        public SubmittedApplicationHousekeeper(
            IConfigurationService configurationService,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IAuditApplicationDetailStrategy auditApplicationDetailStrategy,
            IHardDeleteApplicationStrategy hardDeleteApplicationStrategy)
        {
            _configurationService = configurationService;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _auditApplicationDetailStrategy = auditApplicationDetailStrategy;
            _hardDeleteApplicationStrategy = hardDeleteApplicationStrategy;
        }

        public IEnumerable<ApplicationHousekeepingRequest> GetHousekeepingRequests()
        {
            var vacancyExpiryDate = GetHousekeepingDate();

            var requests = _apprenticeshipApplicationReadRepository
                .GetApplicationsSubmittedOnOrBefore(vacancyExpiryDate)
                .Select(each => new ApplicationHousekeepingRequest
                {
                    ApplicationId = each,
                    VacancyType = VacancyType.Apprenticeship
                })
                .ToList();

            requests.AddRange(_traineeshipApplicationReadRepository
                .GetApplicationsSubmittedOnOrBefore(vacancyExpiryDate)
                .Select(each => new ApplicationHousekeepingRequest
                {
                    ApplicationId = each,
                    VacancyType = VacancyType.Apprenticeship
                }));

            return requests;
        }

        public void Handle(ApplicationHousekeepingRequest request)
        {
            ApplicationDetail application;
            string auditEventTypeCode;

            switch (request.VacancyType)
            {
                case VacancyType.Apprenticeship:
                    {
                        application = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, false);

                        if (application == null)
                        {
                            return;
                        }

                        auditEventTypeCode = AuditEventTypes.HardDeleteApprenticeshipApplication;
                        break;
                    }

                case VacancyType.Traineeship:
                    {
                        application = _traineeshipApplicationReadRepository.Get(request.ApplicationId, false);

                        if (application == null)
                        {
                            return;
                        }

                        auditEventTypeCode = AuditEventTypes.HardDeleteTraineeshipApplication;
                        break;
                    }

                default:
                    throw new InvalidOperationException(string.Format("Unknown vacancy type: {0}.", request.VacancyType));
            }

            if (application.DateApplied <= GetHousekeepingDate())
            {
                _auditApplicationDetailStrategy.Audit(
                    request.VacancyType,
                    request.ApplicationId,
                    auditEventTypeCode);

                _hardDeleteApplicationStrategy.Delete(
                    request.VacancyType,
                    request.ApplicationId);

                return;
            }

            Succesor.Handle(request);
        }

        public IApplicationHousekeeper Succesor { get; set; }

        #region Helpers

        private DateTime GetHousekeepingDate()
        {
            var housekeepingConfiguration = _configurationService.Get<HousekeepingConfiguration>();
            
            return DateTime.UtcNow.AddHours(
                -housekeepingConfiguration.HousekeepingCycleInHours *
                housekeepingConfiguration.Application.HardDeleteSubmittedApplicationAfterCycles);
        }

        #endregion
    }
}