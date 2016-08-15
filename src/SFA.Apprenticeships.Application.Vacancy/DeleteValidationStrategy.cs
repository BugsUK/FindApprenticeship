namespace SFA.Apprenticeships.Application.Vacancy
{
    using Domain.Entities.Raa.Vacancies;

    public class DeleteValidationStrategy : IDeleteValidationStrategy
    {
        public StrategyResult<VacancySummary> Execute(VacancySummary input)
        {
            if (input.Status != VacancyStatus.Draft)
            {
                return new StrategyResult<VacancySummary>(VacancyManagementServiceCodes.Delete.VacancyInIncorrectState);
            }

            return new StrategyResult<VacancySummary>(VacancyManagementServiceCodes.Delete.Ok);
        }
    }
}