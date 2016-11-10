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

        Standard CreateStandard(Standard standard);

        Standard GetStandardById(int standardId);

        Standard UpdateStandard(Standard standard);

        Sector CreateSector(Sector sector);

        Sector GetSectorById(int sectorId);

        Sector UpdateSector(Sector sector);
    }
}