namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System.Collections.Generic;

    public class EmployerEqualityComparer : IEqualityComparer<Employer>
    {
        public bool Equals(Employer x, Employer y)
        {
            if (x == null && y == null) return true;
            return x?.EmployerId == y?.EmployerId;
        }

        public int GetHashCode(Employer obj)
        {
            return obj.EmployerId;
        }
    }
}