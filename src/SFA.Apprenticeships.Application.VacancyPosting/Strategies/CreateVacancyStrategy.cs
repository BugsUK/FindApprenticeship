namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;

    public class CreateVacancyStrategy : ICreateVacancyStrategy
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUpsertVacancyStrategy _upsertVacancyStrategy;

        public CreateVacancyStrategy(IVacancyWriteRepository vacancyWriteRepository, IProviderUserReadRepository providerUserReadRepository, ICurrentUserService currentUserService, IUpsertVacancyStrategy upsertVacancyStrategy)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
            _providerUserReadRepository = providerUserReadRepository;
            _currentUserService = currentUserService;
            _upsertVacancyStrategy = upsertVacancyStrategy;
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
                var vacancyManager = _providerUserReadRepository.GetByUsername(username);

                if (vacancyManager?.PreferredProviderSiteId != null)
                {
                    vacancy.VacancyManagerId = vacancy.DeliveryOrganisationId = vacancyManager.PreferredProviderSiteId.Value;
                }
            }

            // Always set VacancySource as Raa when creating a vacancy from Raa
            vacancy.VacancySource = VacancySource.Raa;

            return _upsertVacancyStrategy.UpsertVacancy(vacancy, v => _vacancyWriteRepository.Create(v));
        }
    }
}