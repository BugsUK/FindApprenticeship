namespace FrameworkDataImport
{
    using System.Linq;

    public static class CategoryExtensions
    {
        public static Category ToCategory(this SFA.Apprenticeships.Domain.Entities.ReferenceData.Category category)
        {
            var newCategory = new Category
            {
                FullName = category.FullName,
                CodeName = category.CodeName,
                SubCategories = category.SubCategories == null ? null : category.SubCategories.Select(c => c.ToCategory()).ToList()
            };
            return newCategory;
        }

        public static Category ToCategory(this Framework framework)
        {
            var name = framework.Name;
            if (framework.IssuingAuthority == "Lantra")
            {
                name = string.Format("{0} ({1})", name, framework.IssuingAuthority);
            }

            var newCategory = new Category
            {
                FullName = name,
                CodeName = framework.Number.ToString(),
                Levels = framework.Levels
            };
            return newCategory;
        }
    }
}