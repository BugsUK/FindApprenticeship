namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;
    using System.Linq;

    public class Category
    {
        public string FullName { get; set; }

        public string CodeName { get; set; }

        public string ParentCategoryCodeName { get; set; }

        public IList<Category> SubCategories { get; set; }

        public long? Count { get; set; }

        public static readonly Category UnknownSectorSubjectAreaTier1 = new Category
        {
            CodeName = $"{CategoryPrefixes.SectorSubjectAreaTier1}UNKNOWN",
            FullName = "Unknown Sector Subject Area Tier 1"
        };

        public static readonly Category InvalidSectorSubjectAreaTier1 = new Category
        {
            CodeName = $"{CategoryPrefixes.SectorSubjectAreaTier1}INVALID",
            FullName = "Invalid Sector Subject Area Tier 1"
        };

        public static readonly Category UnknownFramework = new Category
        {
            CodeName = $"{CategoryPrefixes.Framework}UNKNOWN",
            FullName = "Unknown Framework"
        };

        public static readonly Category InvalidFramework = new Category
        {
            CodeName = $"{CategoryPrefixes.Framework}INVALID",
            FullName = "Invalid Framework"
        };

        public static readonly Category UnknownStandardSector = new Category
        {
            CodeName = $"{CategoryPrefixes.StandardSector}UNKNOWN",
            FullName = "Unknown Standard Sector"
        };

        public static readonly Category InvalidStandardSector = new Category
        {
            CodeName = $"{CategoryPrefixes.StandardSector}INVALID",
            FullName = "Invalid Standard Sector"
        };

        public static readonly Category InvalidSector = new Category
        {
            CodeName = $"{CategoryPrefixes.Sector}INVALID",
            FullName = "Invalid Sector"
        };
    }
}
