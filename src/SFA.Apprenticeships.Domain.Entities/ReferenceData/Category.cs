namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;

    public class Category
    {
        private Category()
        {
            
        }

        public Category(string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, IList<Category> subCategories, long? count)
        {
            CodeName = codeName;
            FullName = fullName;
            ParentCategoryCodeName = parentCategoryCodeName;
            CategoryType = categoryType;
            SubCategories = subCategories ?? new List<Category>();
            Count = count ?? 0;
        }

        public Category(string codeName, string fullName, CategoryType categoryType) : this(codeName, fullName, null, categoryType, null, null)
        {
            
        }

        public Category(string codeName, string fullName, CategoryType categoryType, IList<Category> subCategories) : this(codeName, fullName, null, categoryType, subCategories, null)
        {
            
        }

        public Category(string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType) : this(codeName, fullName, parentCategoryCodeName, categoryType, null, null)
        {
            
        }

        public Category(string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, IList<Category> subCategories) : this(codeName, fullName, parentCategoryCodeName, categoryType, subCategories, null)
        {

        }

        public Category(string codeName, string fullName, CategoryType categoryType, IList<Category> subCategories, long? count) : this(codeName, fullName, null, categoryType, subCategories, count)
        {

        }

        public Category(string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, long? count) : this(codeName, fullName, parentCategoryCodeName, categoryType, null, count)
        {

        }

        public string FullName { get; private set; }

        public string CodeName { get; private set; }

        public string ParentCategoryCodeName { get; private set; }

        public CategoryType CategoryType { get; private set; }

        public IList<Category> SubCategories { get; private set; }

        public long Count { get; set; }

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

        public static readonly Category EmptySectorSubjectAreaTier1 = new Category();

        public static readonly Category EmptyFramework = new Category();
    }
}
