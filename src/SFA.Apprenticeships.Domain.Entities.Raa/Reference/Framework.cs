namespace SFA.Apprenticeships.Domain.Entities.Raa.Reference
{
    using ReferenceData;

    public class Framework
    {
        public int Id { get; set; }

        public string CodeName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string ParentCategoryCodeName { get; set; }

        public Category ToCategory()
        {
            return new Category(CategoryPrefixes.GetFrameworkCode(CodeName), FullName, CategoryPrefixes.GetSectorSubjectAreaTier1Code(ParentCategoryCodeName), CategoryType.Framework);
        }
    }
}
