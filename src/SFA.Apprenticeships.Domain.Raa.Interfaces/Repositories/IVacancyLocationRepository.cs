namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Locations;

    public interface IVacancyLocationReadRepository
    {
        List<VacancyLocation> GetForVacancyId(int vacancyId);

        List<VacancyLocation> GetForVacancyReferenceNumber(int vacancyReferenceNumber);

        IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyIds);
    }

    public interface IVacancyLocationWriteRepository
    {
        List<VacancyLocation> Create(List<VacancyLocation> locationAddresses);
        List<VacancyLocation> Update(List<VacancyLocation> locationAddresses);
        void DeleteFor(int vacancyId);
    }
}