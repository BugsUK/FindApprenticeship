namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;

    public interface IGetVacancySummaryStrategies
    {
        IList<VacancySummary> GetWithStatus(VacancySummaryByStatusQuery query, out int totalRecords);

        IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds);

        IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query);
    }
}