namespace SFA.Apprenticeships.Application.VacancyPosting.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;

    public interface IVacancyLocationsStrategies
    {
        List<VacancyLocation> GetVacancyLocations(int vacancyId);

        List<VacancyLocation> SaveVacancyLocations(List<VacancyLocation> vacancyLocations);

        void DeleteVacancyLocationsFor(int vacancyId);

        IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyPartyIds);
    }
}