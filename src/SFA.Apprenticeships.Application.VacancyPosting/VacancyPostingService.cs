namespace SFA.Apprenticeships.Application.VacancyPosting
{
    //TODO: rename project to SFA.Management.Application.VacancyPosting?
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Interfaces.VacancyPosting;
    using Strategies;

    public class VacancyPostingService : IVacancyPostingService
    {
        private readonly ICreateVacancyStrategy _createVacancyStrategy;
        private readonly IUpdateVacancyStrategy _updateVacancyStrategy;
        private readonly IArchiveVacancyStrategy _archiveVacancyStrategy;
        private readonly IGetNextVacancyReferenceNumberStrategy _getNextVacancyReferenceNumberStrategy;
        private readonly IGetVacancyStrategies _getVacancyStrategies;
        private readonly IGetVacancySummaryStrategies _getVacancySummaryStrategies;
        private readonly IQaVacancyStrategies _qaVacancyStrategies;
        private readonly IVacancyLocationsStrategies _vacancyLocationsStrategies;

        public VacancyPostingService(
            ICreateVacancyStrategy createVacancyStrategy,
            IUpdateVacancyStrategy updateVacancyStrategy,
            IArchiveVacancyStrategy archiveVacancyStrategy,
            IGetNextVacancyReferenceNumberStrategy getNextVacancyReferenceNumberStrategy,
            IGetVacancyStrategies getVacancyStrategies,
            IGetVacancySummaryStrategies getVacancySummaryStrategies,
            IQaVacancyStrategies qaVacancyStrategies,
            IVacancyLocationsStrategies vacancyLocationsStrategies)
        {
            _createVacancyStrategy = createVacancyStrategy;
            _updateVacancyStrategy = updateVacancyStrategy;
            _archiveVacancyStrategy = archiveVacancyStrategy;
            _getNextVacancyReferenceNumberStrategy = getNextVacancyReferenceNumberStrategy;
            _getVacancyStrategies = getVacancyStrategies;
            _getVacancySummaryStrategies = getVacancySummaryStrategies;
            _qaVacancyStrategies = qaVacancyStrategies;
            _vacancyLocationsStrategies = vacancyLocationsStrategies;
        }

        public Vacancy CreateVacancy(Vacancy vacancy)
        {
            return _createVacancyStrategy.CreateVacancy(vacancy);
        }

        public Vacancy UpdateVacancy(Vacancy vacancy)
        {
            return _updateVacancyStrategy.UpdateVacancy(vacancy);
        }

        public Vacancy ArchiveVacancy(Vacancy vacancy)
        {
            return _archiveVacancyStrategy.ArchiveVacancy(vacancy);
        }

        public int GetNextVacancyReferenceNumber()
        {
            return _getNextVacancyReferenceNumberStrategy.GetNextVacancyReferenceNumber();
        }

        public Vacancy GetVacancyByReferenceNumber(int vacancyReferenceNumber)
        {
            return _getVacancyStrategies.GetVacancyByReferenceNumber(vacancyReferenceNumber);
        }

        public Vacancy GetVacancy(Guid vacancyGuid)
        {
            return _getVacancyStrategies.GetVacancyByGuid(vacancyGuid);
        }

        public Vacancy GetVacancy(int vacancyId)
        {
            return _getVacancyStrategies.GetVacancyById(vacancyId);
        }

        public IList<VacancySummary> GetWithStatus(VacancySummaryByStatusQuery query, out int totalRecords)
        {
            return _getVacancySummaryStrategies.GetByStatus(query, out totalRecords);
        }

        public IReadOnlyList<VacancySummary> GetVacancySummariesByIds(IEnumerable<int> vacancyIds)
        {
            return _getVacancySummaryStrategies.GetVacancySummariesByIds(vacancyIds);
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            return _getVacancySummaryStrategies.GetByOwnerPartyIds(ownerPartyIds);
        }

        public Vacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            return _qaVacancyStrategies.ReserveVacancyForQa(vacancyReferenceNumber);
        }

        public void UnReserveVacancyForQa(int vacancyReferenceNumber)
        {
            _qaVacancyStrategies.UnReserveVacancyForQa(vacancyReferenceNumber);
        }

        public List<VacancyLocation> GetVacancyLocations(int vacancyId)
        {
            return _vacancyLocationsStrategies.GetVacancyLocations(vacancyId);
        }

        public List<VacancyLocation> SaveVacancyLocations(List<VacancyLocation> vacancyLocations)
        {
            return _vacancyLocationsStrategies.SaveVacancyLocations(vacancyLocations);
        }

        public void DeleteVacancyLocationsFor(int vacancyId)
        {
            _vacancyLocationsStrategies.DeleteVacancyLocationsFor(vacancyId);
        }

        public IReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>> GetMinimalVacancyDetails(IEnumerable<int> vacancyPartyIds, int providerId, IEnumerable<int> providerSiteIds)
        {
            return _getVacancySummaryStrategies.GetMinimalVacancyDetails(vacancyPartyIds, providerId, providerSiteIds);
        }

        public IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyPartyIds)
        {
            return _vacancyLocationsStrategies.GetVacancyLocationsByVacancyIds(vacancyPartyIds);
        }

        public IList<RegionalTeamMetrics> GetRegionalTeamsMetrics(VacancySummaryByStatusQuery query)
        {
            return _getVacancySummaryStrategies.GetRegionalTeamMetrics(query);
        }
    }
}
