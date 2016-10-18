namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System.Collections.Generic;

    public class VacancyOwnerRelationshipEqualityComparer : IEqualityComparer<VacancyOwnerRelationship>
    {
        public bool Equals(VacancyOwnerRelationship x, VacancyOwnerRelationship y)
        {
            if (x == null && y == null) return true;
            return x?.EmployerId == y?.EmployerId;
        }

        public int GetHashCode(VacancyOwnerRelationship obj)
        {
            return obj.EmployerId;
        }
    }
}