namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;
    using Interfaces.Providers;

    public class AuthoriseCurrentUserStrategy : IAuthoriseCurrentUserStrategy
    {
        private readonly IProviderVacancyAuthorisationService _providerVacancyAuthorisationService;

        public AuthoriseCurrentUserStrategy(IProviderVacancyAuthorisationService providerVacancyAuthorisationService)
        {
            _providerVacancyAuthorisationService = providerVacancyAuthorisationService;
        }

        public Vacancy AuthoriseCurrentUser(Vacancy vacancy)
        {
            if (vacancy != null)
            {
                _providerVacancyAuthorisationService.Authorise(vacancy);
            }

            return vacancy;
        }
    }
}