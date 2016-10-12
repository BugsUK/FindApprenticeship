namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Vacancies;
    using Queries;
    using System;
    using System.Collections.Generic;

    public interface IVacancyReadRepository
    {
        Vacancy Get(int vacancyId);

        Vacancy GetByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetByVacancyGuid(Guid vacancyGuid);

        List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        List<VacancySummary> GetByOwnerPartyId(int ownerPartyId);

        int CountWithStatus(params VacancyStatus[] desiredStatuses);

        List<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount);

        IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyOwnerRelationshipIds, int providerId, IEnumerable<int> providerSiteIds);

        int GetVacancyIdByReferenceNumber(int vacancyReferenceNumber);
    }
}
