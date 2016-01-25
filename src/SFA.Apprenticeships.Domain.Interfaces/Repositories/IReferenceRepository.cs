namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Reference;

    public interface IReferenceRepository
    {
        IList<County> GetCounties();

        IList<Region> GetRegions();

        IList<LocalAuthority> GetLocalAuthorities();
    }
}