namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates.Configuration;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    using Strategies;

    public class SubmittedApplicationHousekeeper : ISubmittedApplicationHousekeeper
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IHardDeleteApplicationStrategy _hardDeleteApplicationStrategy;

        public SubmittedApplicationHousekeeper(
            ILogService logService,
            IConfigurationService configurationService,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IHardDeleteApplicationStrategy hardDeleteApplicationStrategy)
        {
            _logService = logService;
            _configurationService = configurationService;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
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
                    VacancyType = VacancyType.Traineeship
                }));

            return requests;
        }

        public void Handle(ApplicationHousekeepingRequest request)
        {
            ApplicationDetail application;

            switch (request.VacancyType)
            {
                case VacancyType.Apprenticeship:
                    {
                        application = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, false);

                        if (application == null)
                        {
                            _logService.Debug("Apprenticeship application no longer exists, no housekeeping to do for id={0}",
                                request.ApplicationId);
                            return;
                        }
                        break;
                    }

                case VacancyType.Traineeship:
                    {
                        application = _traineeshipApplicationReadRepository.Get(request.ApplicationId, false);

                        if (application == null)
                        {
                            _logService.Debug("Traineeship application no longer exists, no housekeeping to do for id={0}",
                                request.ApplicationId);
                            return;
                        }
                        break;
                    }

                default:
                    throw new InvalidOperationException(string.Format("Unknown vacancy type: {0}.", request.VacancyType));
            }

            if (application.DateApplied.HasValue && application.DateApplied <= GetHousekeepingDate())
            {
                _logService.Info("Deleting submitted application: type={0}, id={1}, date applied={2}",
                    request.VacancyType, request.ApplicationId, application.DateApplied);

                _hardDeleteApplicationStrategy.Delete(
                    request.VacancyType,
                    request.ApplicationId);

                _logService.Info("Deleted submitted application: type={0}, id={1}, date applied={2}",
                    request.VacancyType, request.ApplicationId, application.DateApplied);
                return;
            }

            Successor.Handle(request);
        }

        public IApplicationHousekeeper Successor { get; set; }

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