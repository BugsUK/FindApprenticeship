namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyLockingService
    {
        bool CanBeReservedForQABy(string userName, VacancySummary vacancySummary);
    }
}