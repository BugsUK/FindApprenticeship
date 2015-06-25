namespace FrameworkDataImport.Entities
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category x, Category y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.CodeName, y.CodeName);
        }

        public int GetHashCode(Category obj)
        {
            unchecked
            {
                return (obj.CodeName != null ? obj.CodeName.GetHashCode() : 0);
            }
        }
    }
}