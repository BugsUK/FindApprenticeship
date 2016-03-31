namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    using Domain.Entities.ReferenceData;

    internal class Framework
    {
        internal int ApprenticeshipFrameworkId { get; set; }

        internal int ApprenticeshipOccupationId { get; set; }

        internal string ParentCategoryCodeName { get; set; }

        internal string CodeName { get; set; }

        internal string FullName { get; set; }

        internal string ShortName { get; set; }

        /// <summary>
        /// TODO: Find a better way to do this!
        /// </summary>
        /// <returns></returns>
        internal Category ToCategory()
        {
            return new Category(ApprenticeshipFrameworkId, CategoryPrefixes.GetFrameworkCode(CodeName), FullName, CategoryPrefixes.GetSectorSubjectAreaTier1Code(ParentCategoryCodeName), CategoryType.Framework);
        }
    }
}
