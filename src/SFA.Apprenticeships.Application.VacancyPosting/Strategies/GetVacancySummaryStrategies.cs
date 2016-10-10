namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
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
            return _vacancyReadRepository.GetByIds(vacancyIds);
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            return _vacancyReadRepository.GetByOwnerPartyIds(ownerPartyIds);
        }

        public IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds, int providerId, IEnumerable<int> providerSiteIds)
        {
            return _vacancyReadRepository.GetMinimalVacancyDetails(vacancyPartyIds, providerId, providerSiteIds);
        }

        public IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query)
        {
            return _vacancySummaryService.GetRegionalTeamMetrics(query);
        }
    }
}