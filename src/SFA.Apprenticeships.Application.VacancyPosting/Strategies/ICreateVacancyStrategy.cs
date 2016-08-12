namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface ICreateVacancyStrategy
    {
        Vacancy CreateVacancy(Vacancy vacancy);
    }
}