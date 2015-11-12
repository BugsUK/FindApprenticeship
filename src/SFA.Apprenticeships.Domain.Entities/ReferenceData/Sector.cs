namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;

    public class Sector
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Standard> Standards { get; set; }
    }
}