namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System.Collections.Generic;

    public class StandardSubjectAreaTierOne
    {
        //TODO Add Name and StandardSector properties to represent the complete hierachy
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Sector> Sectors { get; set; }
    }
}