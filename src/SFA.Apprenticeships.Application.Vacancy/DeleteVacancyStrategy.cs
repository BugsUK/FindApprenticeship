namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Presentation;
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

            if (!input.Status.IsStateDeletable())
            {
                return new StrategyResult(VacancyManagementServiceCodes.Delete.VacancyInIncorrectState);
            }

            _writeRepository.Delete(input.VacancyId);

            return new StrategyResult(VacancyManagementServiceCodes.Delete.Ok);
        }
    }
}