namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IArchiveVacancyStrategy
    {
        Vacancy ArchiveVacancy(Vacancy vacancy);
    }
}