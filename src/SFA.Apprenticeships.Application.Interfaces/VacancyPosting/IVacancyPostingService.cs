namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using System;
    using System.Collections.Generic;
	using Domain.Raa.Interfaces.Repositories.Models;

    public interface IVacancyPostingService
    {
        Vacancy CreateVacancy(Vacancy vacancy);

        int GetNextVacancyReferenceNumber();

        Vacancy GetVacancy(int vacancyId);

        Vacancy GetVacancyByReferenceNumber(int vacancyReferenceNumber);

        Vacancy GetVacancy(Guid vacancyGuid);
        
        IList<VacancySummary> GetWithStatus(VacancySummaryByStatusQuery query, out int totalRecords);

        IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds);

        List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds);

        Vacancy ReserveVacancyForQA(int vacancyReferenceNumber);

        void UnReserveVacancyForQa(int vacancyReferenceNumber);

        List<VacancyLocation> GetVacancyLocations(int vacancyId);

        List<VacancyLocation> CreateVacancyLocations(List<VacancyLocation> vacancyLocations);

        List<VacancyLocation> UpdateVacancyLocations(List<VacancyLocation> vacancyLocations);

        void DeleteVacancyLocationsFor(int vacancyId);

        Vacancy UpdateVacancy(Vacancy vacancy);

        Vacancy ArchiveVacancy(Vacancy vacancy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyOwnerRelationshipIds"></param>
        /// <param name="providerId"></param>
        /// <returns>VacancyOwnerRelationshipId => IVacancyIdStatusAndClosingDate</returns>
        IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyOwnerRelationshipIds, int providerId, IEnumerable<int> providerSiteIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyOwnerRelationshipIds"></param>
        /// <returns>VacancyPartId => VacancyLocation</returns>
        IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyOwnerRelationshipIds);

        Vacancy UpdateVacanciesWithNewProvider(Vacancy vacancies);
		
        IList<RegionalTeamMetrics> GetRegionalTeamsMetrics(VacancySummaryByStatusQuery query);
    }
}
