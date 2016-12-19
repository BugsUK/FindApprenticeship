namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Service;
    using Application.Vacancy;
    using Domain.Entities.Raa.Vacancies;

    public class VacancyManagementProvider : IVacancyManagementProvider
    {
        private readonly IVacancyManagementService _vacancyManagementService;

        public VacancyManagementProvider(IVacancyManagementService vacancyManagementService)
        {
            _vacancyManagementService = vacancyManagementService;
        }

        public IServiceResult Delete(int vacancyId)
        {
            return new ServiceResult(_vacancyManagementService.Delete(vacancyId).Code);
        }

        public IServiceResult<VacancySummary> FindSummary(int vacancyId)
        {
            return _vacancyManagementService.FindSummary(vacancyId);
        }

        public IServiceResult<VacancySummary> FindSummaryByReferenceNumber(int vacancyReferenceNumber)
        {
            return _vacancyManagementService.FindSummaryByReferenceNumber(vacancyReferenceNumber);
        }
    }
}