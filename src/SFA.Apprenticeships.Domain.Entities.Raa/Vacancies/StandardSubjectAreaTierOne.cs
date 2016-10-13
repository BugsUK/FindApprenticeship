namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System.Collections.Generic;

    public class StandardSubjectAreaTierOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
    }
}