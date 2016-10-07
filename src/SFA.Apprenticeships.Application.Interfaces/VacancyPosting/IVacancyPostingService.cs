namespace SFA.Apprenticeships.Application.Interfaces.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
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

        List<VacancyLocation> SaveVacancyLocations(List<VacancyLocation> vacancyLocations);

        void DeleteVacancyLocationsFor(int vacancyId);

        Vacancy UpdateVacancy(Vacancy vacancy);

        Vacancy ArchiveVacancy(Vacancy vacancy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyPartyIds"></param>
        /// <param name="providerId"></param>
        /// <returns>VacancyPartyId => IVacancyIdStatusAndClosingDate</returns>
        IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds, int providerId, IEnumerable<int> providerSiteIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyPartyIds"></param>
        /// <returns>VacancyPartId => VacancyLocation</returns>
        IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyPartyIds);


        IList<RegionalTeamMetrics> GetRegionalTeamsMetrics(VacancySummaryByStatusQuery query);
    }
}
