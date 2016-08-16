namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    using Domain.Entities.Raa.Vacancies;

    public interface IProviderVacancyAuthorisationService
    {
        void Authorise(Vacancy vacancy);
    }
}
