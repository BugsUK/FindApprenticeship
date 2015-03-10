namespace SFA.Apprenticeships.Application.Vacancies
{
    using Entities;

    public interface IVacancyIndexDataProvider
    {
        int GetVacancyPageCount();

        VacancySummaries GetVacancySummaries(int pageNumber);
    }
}
