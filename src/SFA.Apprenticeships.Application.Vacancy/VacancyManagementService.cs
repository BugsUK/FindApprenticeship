namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Service;

    public class VacancyManagementService : IVacancyManagementService
    {
        private readonly IVacancyReadRepository _readRepository;
        private readonly IVacancySummaryService _vacancySummaryService;
        private readonly IDeleteVacancyStrategy _deleteVacancyStrategy;

        public VacancyManagementService(IVacancyReadRepository readRepository, 
            IDeleteVacancyStrategy deleteVacancyStrategy,
            IVacancySummaryService vacancySummaryService)
        {
            _readRepository = readRepository;
            _deleteVacancyStrategy = deleteVacancyStrategy;
            _vacancySummaryService = vacancySummaryService;
        }

        public IServiceResult Delete(int vacancyId)
        {
            var summary = _vacancySummaryService.GetById(vacancyId);
            var result = _deleteVacancyStrategy.Execute(summary);

            return new ServiceResult(result.Code);
        }

        public IServiceResult<VacancySummary> FindSummary(int vacancyId)
        {
            var vacancySummary = _vacancySummaryService.GetById(vacancyId);
            if (vacancySummary == null)
            {
                return new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.NotFound, null);
            }

            return new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.Ok, vacancySummary);
        }
    }
}