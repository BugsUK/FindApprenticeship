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

        County GetCounty(int countyId);

        County GetCounty(string countyName);

        IEnumerable<LocalAuthority> GetLocalAuthorities();

        LocalAuthority GetLocalAuthority(int localAuthorityId);

        LocalAuthority GetLocalAuthority(string localAuthorityCodeName);

        void UpdateStandard(Standard standard);
    }
}