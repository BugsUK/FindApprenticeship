namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Service;
    using Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;

    public interface IVacancyManagementProvider
    {
        IServiceResult Delete(int vacancyReferenceNumber);
        IServiceResult<VacancySummary> FindSummary(int vacancyReferenceNumber);
    }
}