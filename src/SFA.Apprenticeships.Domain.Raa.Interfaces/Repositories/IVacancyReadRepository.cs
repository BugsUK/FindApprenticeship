namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Raa.Vacancies;
    using Queries;

    public interface IVacancyReadRepository
    {
        Vacancy Get(int vacancyId);

        Vacancy GetByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetByVacancyGuid(Guid vacancyGuid);
        
        List<VacancySummary> GetByIds(IEnumerable<int> vacancyIds);

        List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        int CountWithStatus(params VacancyStatus[] desiredStatuses);

        List<VacancySummary> GetWithStatus(int pageSize, int page, bool filterByProviderBeenMigrated, params VacancyStatus[] desiredStatuses);

        List<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);

        IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds);
    }
}
