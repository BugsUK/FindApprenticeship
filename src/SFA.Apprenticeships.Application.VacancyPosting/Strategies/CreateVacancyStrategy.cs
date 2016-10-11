namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Providers;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    public class CreateVacancyStrategy : ICreateVacancyStrategy
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUpsertVacancyStrategy _upsertVacancyStrategy;
        private readonly IProviderService _providerService;

        public CreateVacancyStrategy(IVacancyWriteRepository vacancyWriteRepository, IProviderUserReadRepository providerUserReadRepository, ICurrentUserService currentUserService, IUpsertVacancyStrategy upsertVacancyStrategy, IProviderService providerService)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
            _providerUserReadRepository = providerUserReadRepository;
            _currentUserService = currentUserService;
            _upsertVacancyStrategy = upsertVacancyStrategy;
            _providerService = providerService;
        }

        public Vacancy CreateVacancy(Vacancy vacancy)
        {
            Condition.Requires(vacancy);

            if (vacancy.Status == VacancyStatus.Completed)
            {
                var message = $"Vacancy {vacancy.VacancyReferenceNumber} can not be in Completed status on saving.";
                throw new CustomException(message, ErrorCodes.EntityStateError);
            }

            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var userProfile = _providerUserReadRepository.GetByUsername(username);

                var ukprn = _currentUserService.GetClaimValue("ukprn");
                var ukprnoverride = _currentUserService.GetClaimValue("ukprnoverride");
                if (!string.IsNullOrEmpty(ukprnoverride))
                {
                    ukprn = ukprnoverride;
                }

                //Ensure that the provider site id assigned to the vacancy is valid for the current user and their (potentially impersonated) provider
                var providerSites = _providerService.GetProviderSites(ukprn).ToList();
                var providerSiteId = userProfile.PreferredProviderSiteId ?? 0;
                if (providerSites.All(ps => ps.ProviderSiteId != providerSiteId))
                {
                    providerSiteId = providerSites.First().ProviderSiteId;
                }

                vacancy.VacancyManagerId = vacancy.DeliveryOrganisationId = providerSiteId;
            }

            // Always set VacancySource as Raa when creating a vacancy from Raa
            vacancy.VacancySource = VacancySource.Raa;

            return _upsertVacancyStrategy.UpsertVacancy(vacancy, v => _vacancyWriteRepository.Create(v));
        }
    }
}