namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System.Collections.Generic;
    using Locations;

    public class EmployerVerifiedOrganisationComparer : IEqualityComparer<Employer>
    {
        private readonly IEqualityComparer<PostalAddress> _postalAddressOnlyComparer = new PostalAddressOnlyComparer();

        public bool Equals(Employer x, Employer y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(x, y)) return true;
            return string.Equals(x.EdsUrn, y.EdsUrn) && string.Equals(x.FullName, y.FullName) && string.Equals(x.TradingName, y.TradingName) && _postalAddressOnlyComparer.Equals(x.Address, y.Address);
        }

        public int GetHashCode(Employer obj)
        {
            unchecked
            {
                var hashCode = (obj.FullName != null ? obj.FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.TradingName != null ? obj.TradingName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Address != null ? _postalAddressOnlyComparer.GetHashCode(obj.Address) : 0);
                return hashCode;
            }
        }
    }
}