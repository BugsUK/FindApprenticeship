namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Service;

    public class VacancyManagementService : IVacancyManagementService
    {
        private readonly IVacancyWriteRepository _writeRepository;
        private readonly IVacancyReadRepository _readRepository;
        private readonly IDeleteValidationStrategy _deleteValidationStrategy;

        public VacancyManagementService(IVacancyWriteRepository writeRepository, IVacancyReadRepository readRepository, IDeleteValidationStrategy deleteValidationStrategy)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _deleteValidationStrategy = deleteValidationStrategy;
        }

        public IServiceResult Delete(int vacancyId)
        {
            var summary =_readRepository.GetById(vacancyId);
            var result = _deleteValidationStrategy.Execute(summary);

            if (result.Code == VacancyManagementServiceCodes.Delete.Ok)
            {
                _writeRepository.Delete(vacancyId);
            }

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