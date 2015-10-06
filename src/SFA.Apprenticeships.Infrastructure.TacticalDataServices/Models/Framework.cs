namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    using Domain.Entities.ReferenceData;

    internal class Framework
    {
        internal int ApprenticeshipOccupationId { get; set; }

        internal string ParentCategoryCodeName { get; set; }

        internal string CodeName { get; set; }

        internal string FullName { get; set; }

        internal string ShortName { get; set; }

        internal Category ToCategory()
        {
            return new Category
            {
                CodeName = CodeName,
                FullName = FullName,
                ParentCategoryCodeName = ParentCategoryCodeName
            };
        }
    }
}
