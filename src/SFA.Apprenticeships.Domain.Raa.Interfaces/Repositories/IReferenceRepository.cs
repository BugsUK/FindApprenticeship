namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Reference;
    using Entities.Raa.Vacancies;
    using Entities.ReferenceData;

    public interface IReferenceRepository
    {
        IList<Framework> GetFrameworks();

        IList<Occupation> GetOccupations();

        IList<Standard> GetStandards();

        IList<Sector> GetSectors();

        IList<ReleaseNote> GetReleaseNotes();

        IList<StandardSubjectAreaTierOne> GetStandardSubjectAreaTierOnes();

        IEnumerable<County> GetCounties();

        County GetCountyById(int countyId);

        County GetCountyByCode(string countyCode);

        County GetCountyByName(string countyName);

        IEnumerable<LocalAuthority> GetLocalAuthorities();

        LocalAuthority GetLocalAuthorityById(int localAuthorityId);

        LocalAuthority GetLocalAuthorityByCode(string localAuthorityCode);

        IEnumerable<Region> GetRegions();

        Region GetRegionById(int regionId);

        Region GetRegionByCode(string regionCode);
		
		void UpdateStandard(Standard standard);
    }
}