namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;

    public interface IGetVacancySummaryStrategies
    {
        List<VacancySummary> GetWithStatus(params VacancyStatus[] desiredStatuses);

        IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds);

        List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds, int providerId);
    }
}