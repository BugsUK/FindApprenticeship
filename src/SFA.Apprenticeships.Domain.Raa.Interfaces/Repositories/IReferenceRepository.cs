namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Reference;
    using Entities.Raa.Vacancies;

    public interface IReferenceRepository
    {
        IList<County> GetCounties();

        //IList<Region> GetRegions();

        IList<LocalAuthority> GetLocalAuthorities();

        IList<Framework> GetFrameworks();

        IList<Occupation> GetOccupations();

        IList<Standard> GetStandards();

        IList<Sector> GetSectors();
    }
}