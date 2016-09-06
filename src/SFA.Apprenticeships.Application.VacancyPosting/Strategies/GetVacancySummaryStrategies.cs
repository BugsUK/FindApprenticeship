namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;

    public class GetVacancySummaryStrategies : IGetVacancySummaryStrategies
    {
        private readonly IVacancyReadRepository _vacancyReadRepository;

        public GetVacancySummaryStrategies(IVacancyReadRepository vacancyReadRepository)
        {
            _vacancyReadRepository = vacancyReadRepository;
        }

        public List<VacancySummary> GetWithStatus(params VacancyStatus[] desiredStatuses)
        {
            return _vacancyReadRepository.GetWithStatus(0, 0, true, desiredStatuses);
        }

        public IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds)
        {
            return _vacancyReadRepository.GetByIds(vacancyIds);
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            return _vacancyReadRepository.GetByOwnerPartyIds(ownerPartyIds);
        }

        public IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds, int providerId)
        {
            return _vacancyReadRepository.GetMinimalVacancyDetails(vacancyPartyIds, providerId);
        }
    }
}