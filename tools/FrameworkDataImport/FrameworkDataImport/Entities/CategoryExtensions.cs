namespace FrameworkDataImport.Entities
{
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public static class CategoryExtensions
    {
        public static Category ToCategory(this Framework framework, string parentCategoryCodeName)
        {
            var newCategory = new Category
            {
                FullName = framework.Name,
                CodeName = framework.Number.ToString(),
                ParentCategoryCodeName = parentCategoryCodeName,
                Levels = framework.Levels
            };
            return newCategory;
        }
    }
}