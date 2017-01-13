namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Interfaces.Service;

    public interface IVacancyManagementService
    {
        IServiceResult Delete(int vacancyId);
        IServiceResult<VacancySummary> FindSummary(int vacancyId);
        IServiceResult<VacancySummary> FindSummaryByReferenceNumber(int vacancyReferenceNumber);
    }
}