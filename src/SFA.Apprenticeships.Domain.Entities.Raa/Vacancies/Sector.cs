namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System.Collections.Generic;

    public class Sector
    {
        public int SectorId { get; set; }

        public string Name { get; set; }

        public int ApprenticeshipOccupationId { get; set; }

        public IEnumerable<Standard> Standards { get; set; }
    }
}