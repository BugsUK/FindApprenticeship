namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Service;
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyManagementProvider
    {
        IServiceResult Delete(int vacancyId);
        IServiceResult<VacancySummary> FindSummary(int vacancyId);
        IServiceResult<VacancySummary> FindSummaryByReferenceNumber(int vacancyReferenceNumber);
    }
}