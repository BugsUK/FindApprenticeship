namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Service;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.VacancyManagement;

    public interface IVacancyManagementProvider
    {
        IServiceResult Delete(int vacancyId);
        IServiceResult<VacancySummary> FindSummary(int vacancyId);
        IServiceResult<VacancySummary> FindSummaryByReferenceNumber(int vacancyReferenceNumber);
        Task<IServiceResult<EditWageViewModel>> EditWage(EditWageViewModel editWageViewModel);
    }
}