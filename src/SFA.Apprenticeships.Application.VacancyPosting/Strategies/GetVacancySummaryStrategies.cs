namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Vacancy;

    public class GetVacancySummaryStrategies : IGetVacancySummaryStrategies
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IVacancySummaryService _vacancySummaryService;

        public GetVacancySummaryStrategies(IVacancyReadRepository vacancyReadRepository,
                                            IVacancySummaryService vacancySummaryService)
        {
            _vacancyReadRepository = vacancyReadRepository;
            _vacancySummaryService = vacancySummaryService;
        }

        public IList<VacancySummary> GetWithStatus(VacancySummaryByStatusQuery query, out int totalRecords)
        {
            return _vacancySummaryService.GetWithStatus(query, out totalRecords);
        }

        public IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds)
        {
            return _vacancySummaryService.GetByIds(vacancyIds).ToList();
        }

        public IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query)
        {
            return _vacancySummaryService.GetRegionalTeamMetrics(query);
        }
    }
}