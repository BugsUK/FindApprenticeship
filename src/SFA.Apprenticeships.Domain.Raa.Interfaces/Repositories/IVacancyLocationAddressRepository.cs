namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Locations;

    public interface IVacancyLocationAddressReadRepository
    {
        List<VacancyLocationAddress> GetForVacancyId(int vacancyId);
    }

    public interface IVacancyLocationAddressWriteRepository
    {
        List<VacancyLocationAddress> Save(List<VacancyLocationAddress> locationAddresses);
    }
}