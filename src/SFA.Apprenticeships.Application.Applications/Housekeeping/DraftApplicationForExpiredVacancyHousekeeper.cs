namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Candidates.Configuration;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Strategies;

    public class DraftApplicationForExpiredVacancyHousekeeper : IDraftApplicationForExpiredVacancyHousekeeper
    {
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IHardDeleteApplicationStrategy _hardDeleteApplicationStrategy;

        public DraftApplicationForExpiredVacancyHousekeeper(
            ILogService logService,
            IConfigurationService configurationService,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IHardDeleteApplicationStrategy hardDeleteApplicationStrategy)
        {
            _logService = logService;
            _configurationService = configurationService;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _hardDeleteApplicationStrategy = hardDeleteApplicationStrategy;
        }

        public IEnumerable<ApplicationHousekeepingRequest> GetHousekeepingRequests()
        {
            var vacancyExpiryDate = GetHousekeepingDate();

            return _apprenticeshipApplicationReadRepository
                .GetDraftApplicationsForExpiredVacancies(vacancyExpiryDate)
                .Select(each => new ApplicationHousekeepingRequest
                {
                    ApplicationId = each,
                    VacancyType = VacancyType.Apprenticeship
                });
        }

        public void Handle(ApplicationHousekeepingRequest request)
        {
            switch (request.VacancyType)
            {
                case VacancyType.Apprenticeship:
                    {
                        var application = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, false);

                        if (application == null)
                        {
                            _logService.Debug("Apprenticeship application no longer exists, no housekeeping to do for id={0}",
                                request.ApplicationId);

                            return;
                        }

                        if (!application.DateApplied.HasValue && application.Vacancy.ClosingDate <= GetHousekeepingDate())
                        {
                            _hardDeleteApplicationStrategy.Delete(
                                request.VacancyType,
                                request.ApplicationId);

                            return;
                        }
                        break;
                    }

                case VacancyType.Traineeship:
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown vacancy type: {0}.", request.VacancyType));
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
                housekeepingConfiguration.Application.HardDeleteDraftApplicationForExpiredVacancyAfterCycles);
        }

        #endregion
    }
}