namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Interfaces;
    using Interfaces.Providers;

    public class UpsertVacancyStrategy : IUpsertVacancyStrategy
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IProviderVacancyAuthorisationService _providerVacancyAuthorisationService;

        public UpsertVacancyStrategy(ICurrentUserService currentUserService, IProviderUserReadRepository providerUserReadRepository, IVacancyReadRepository vacancyReadRepository, IProviderVacancyAuthorisationService providerVacancyAuthorisationService)
        {
            _currentUserService = currentUserService;
            _providerUserReadRepository = providerUserReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _providerVacancyAuthorisationService = providerVacancyAuthorisationService;
        }

        public Vacancy UpsertVacancy(Vacancy vacancy, Func<Vacancy, Vacancy> operation)
        {
            Condition.Requires(vacancy);

            AuthoriseCurrentUser(vacancy);

            if (_currentUserService.IsInRole(Roles.Faa))
            {
                var username = _currentUserService.CurrentUserName;
                var lastEditedBy = _providerUserReadRepository.GetByUsername(username);
                if (lastEditedBy != null)
                {
                    vacancy.LastEditedById = lastEditedBy.ProviderUserId;
                }
            }

            vacancy = operation(vacancy);

            return _vacancyReadRepository.Get(vacancy.VacancyId);
        }

        private void AuthoriseCurrentUser(Vacancy vacancy)
        {
            if (vacancy != null)
            {
                _providerVacancyAuthorisationService.Authorise(vacancy);
            }
        }
    }
}