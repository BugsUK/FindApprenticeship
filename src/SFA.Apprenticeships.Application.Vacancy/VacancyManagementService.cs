namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Service;

    public class VacancyManagementService : IVacancyManagementService
    {
        private readonly IVacancyReadRepository _readRepository;
        private readonly IDeleteVacancyStrategy _deleteVacancyStrategy;

        public VacancyManagementService(IVacancyReadRepository readRepository, IDeleteVacancyStrategy deleteVacancyStrategy)
        {
            _readRepository = readRepository;
            _deleteVacancyStrategy = deleteVacancyStrategy;
        }

        public IServiceResult Delete(int vacancyId)
        {
            var summary =_readRepository.GetById(vacancyId);
            var result = _deleteVacancyStrategy.Execute(summary);

            return new ServiceResult(result.Code);
        }

        public IServiceResult<VacancySummary> FindSummary(int vacancyId)
        {
            var vacancySummary = _readRepository.GetById(vacancyId);
            if (vacancySummary == null)
            {
                return new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.NotFound, null);
            }

            return new ServiceResult<VacancySummary>(VacancyManagementServiceCodes.FindSummary.Ok, vacancySummary);
        }
    }
}