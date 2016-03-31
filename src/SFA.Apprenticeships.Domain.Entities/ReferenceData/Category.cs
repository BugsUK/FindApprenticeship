namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;

    public class Category
    {
        private Category()
        {
            
        }

        public Category(int id, string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, IList<Category> subCategories, long? count)
        {
            Id = id;
            CodeName = codeName;
            FullName = fullName;
            ParentCategoryCodeName = parentCategoryCodeName;
            CategoryType = categoryType;
            SubCategories = subCategories ?? new List<Category>();
            Count = count ?? 0;
        }

        public Category(int id, string codeName, string fullName, CategoryType categoryType) : this(id, codeName, fullName, null, categoryType, null, null)
        {
            
        }

        public Category(int id, string codeName, string fullName, CategoryType categoryType, IList<Category> subCategories) : this(id, codeName, fullName, null, categoryType, subCategories, null)
        {
            
        }

        public Category(int id, string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType) : this(id, codeName, fullName, parentCategoryCodeName, categoryType, null, null)
        {
            
        }

        public Category(int id, string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, IList<Category> subCategories) : this(id, codeName, fullName, parentCategoryCodeName, categoryType, subCategories, null)
        {

        }

        public Category(int id, string codeName, string fullName, CategoryType categoryType, IList<Category> subCategories, long? count) : this(id, codeName, fullName, null, categoryType, subCategories, count)
        {

        }

        public Category(int id, string codeName, string fullName, string parentCategoryCodeName, CategoryType categoryType, long? count) : this(id, codeName, fullName, parentCategoryCodeName, categoryType, null, count)
        {

        }

        public int Id { get; private set; }

        public string FullName { get; private set; }

        public string CodeName { get; private set; }

        public string ParentCategoryCodeName { get; private set; }

        public CategoryType CategoryType { get; private set; }

        public IList<Category> SubCategories { get; private set; }

        public long Count { get; set; }

        public static readonly Category UnknownSectorSubjectAreaTier1 = new Category
        {
            CodeName = CategoryPrefixes.GetSectorSubjectAreaTier1Code("UNKNOWN"),
            FullName = "Unknown Sector Subject Area Tier 1"
        };

        public static readonly Category InvalidSectorSubjectAreaTier1 = new Category
        {
            CodeName = CategoryPrefixes.GetSectorSubjectAreaTier1Code("INVALID"),
            FullName = "Invalid Sector Subject Area Tier 1"
        };

        public static readonly Category EmptySectorSubjectAreaTier1 = new Category();

        public static readonly Category UnknownFramework = new Category
        {
            CodeName = CategoryPrefixes.GetFrameworkCode("UNKNOWN"),
            FullName = "Unknown Framework"
        };

        public static readonly Category InvalidFramework = new Category
        {
            CodeName = CategoryPrefixes.GetFrameworkCode("INVALID"),
            FullName = "Invalid Framework"
        };

        public static readonly Category EmptyFramework = new Category();

        public static readonly Category UnknownStandardSector = new Category
        {
            CodeName = CategoryPrefixes.GetStandardSectorCode("UNKNOWN"),
            FullName = "Unknown Standard Sector"
        };

        public static readonly Category InvalidStandardSector = new Category
        {
            CodeName = CategoryPrefixes.GetStandardSectorCode("INVALID"),
            FullName = "Invalid Standard Sector"
        };

        public static readonly Category InvalidSector = new Category
        {
            CodeName = CategoryPrefixes.GetSectorCode("INVALID"),
            FullName = "Invalid Sector"
        };
    }
}
