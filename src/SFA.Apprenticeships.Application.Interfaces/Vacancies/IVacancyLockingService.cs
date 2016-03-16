namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyLockingService
    {
        bool IsVacancyAvailableToQABy(string userName, VacancySummary vacancySummary);

        VacancySummary GetNextAvailableVacancy(string userName, List<VacancySummary> vacancies);
    }
}