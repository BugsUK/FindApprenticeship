namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Providers;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    public class CreateVacancyStrategy : ICreateVacancyStrategy
    {
        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IUpsertVacancyStrategy _upsertVacancyStrategy;
        private readonly IProviderService _providerService;

        public CreateVacancyStrategy(IVacancyWriteRepository vacancyWriteRepository, IUpsertVacancyStrategy upsertVacancyStrategy, IProviderService providerService)
        {
            _vacancyWriteRepository = vacancyWriteRepository;
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

            //The VacancyManagerId, DeliveryOrganisationId and VacancyOwnerRelationship.ProviderSiteId should match.
            //Due to impersonation and user's changing their default provider site, we can't completely rely on the user's DefaultProviderSiteId so using the VOR value instead
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancy.VacancyOwnerRelationshipId, false);
            vacancy.VacancyManagerId = vacancy.DeliveryOrganisationId = vacancyOwnerRelationship.ProviderSiteId;

            // Always set VacancySource as Raa when creating a vacancy from Raa
            vacancy.VacancySource = VacancySource.Raa;

            return _upsertVacancyStrategy.UpsertVacancy(vacancy, v => _vacancyWriteRepository.Create(v));
        }
    }
}