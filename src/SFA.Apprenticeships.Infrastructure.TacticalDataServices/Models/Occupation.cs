namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    internal class Occupation
    {
        public Occupation()
        {
            Frameworks = new List<Framework>();    
        }

        internal int ApprenticeshipOccupationId { get; set; }

        internal string CodeName { get; set; }

        internal string FullName { get; set; }

        internal string ShortName { get; set; }

        public List<Framework> Frameworks { get; set; }

        internal Category ToCategory()
        {
            var category = new Category($"{CategoryPrefixes.SectorSubjectAreaTier1}{CodeName}", FullName, CategoryType.SectorSubjectAreaTier1);
            Frameworks.ForEach(f => category.SubCategories.Add(f.ToCategory()));
            return category;
        }

    }
}
