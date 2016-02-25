namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System.Collections.Generic;

    public class VacancyPartyEqualityComparer : IEqualityComparer<VacancyParty>
    {
        public bool Equals(VacancyParty x, VacancyParty y)
        {
            if (x == null && y == null) return true;
            return x?.EmployerId == y?.EmployerId;
        }

        public int GetHashCode(VacancyParty obj)
        {
            return obj.EmployerId;
        }
    }
}