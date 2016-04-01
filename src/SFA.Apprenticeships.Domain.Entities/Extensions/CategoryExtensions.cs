namespace SFA.Apprenticeships.Domain.Entities.Extensions
{
    using System.Linq;
    using ReferenceData;

    public static class CategoryExtensions
    {
        public static bool IsValid(this Category category)
        {
            var invalidCategories = new[]
            {
                Category.UnknownSectorSubjectAreaTier1,
                Category.InvalidSectorSubjectAreaTier1,
                Category.UnknownFramework,
                Category.InvalidFramework,
                Category.UnknownStandardSector,
                Category.InvalidStandardSector,
                Category.InvalidSector
            };

            return invalidCategories.All(c => c.CodeName != category.CodeName);
        }
    }
}