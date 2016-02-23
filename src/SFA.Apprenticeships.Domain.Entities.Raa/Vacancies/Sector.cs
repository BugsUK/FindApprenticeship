namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System.Collections.Generic;

    public class Sector
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Standard> Standards { get; set; }
    }
}