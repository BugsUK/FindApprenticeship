namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    using System.Collections.Generic;

    public class Category
    {
        public string FullName { get; set; }

        public string CodeName { get; set; }

        public string ParentCategoryCodeName { get; set; }

        public IList<Category> SubCategories { get; set; }

        public long? Count { get; set; }
    }
}
