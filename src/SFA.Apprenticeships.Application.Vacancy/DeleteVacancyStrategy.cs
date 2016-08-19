namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Service;

    public class DeleteVacancyStrategy : IDeleteVacancyStrategy
    {
        private readonly IVacancyWriteRepository _writeRepository;

        public DeleteVacancyStrategy(IVacancyWriteRepository writeRepository)
        {
            _writeRepository = writeRepository;
        }

        public StrategyResult Execute(VacancySummary input)
        {
            if (input == null)
            {
                return new StrategyResult(VacancyManagementServiceCodes.Delete.VacancyNotFound);
            }

            if (input.Status != VacancyStatus.Draft)
            {
                return new StrategyResult(VacancyManagementServiceCodes.Delete.VacancyInIncorrectState);
            }

            _writeRepository.Delete(input.VacancyId);

            return new StrategyResult(VacancyManagementServiceCodes.Delete.Ok);
        }
    }
}