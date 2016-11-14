namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IUpdateVacancyStrategy
    {
        Vacancy UpdateVacancy(Vacancy vacancy);
        Vacancy UpdateVacancyWithNewProvider(Vacancy vacancy);
    }
}