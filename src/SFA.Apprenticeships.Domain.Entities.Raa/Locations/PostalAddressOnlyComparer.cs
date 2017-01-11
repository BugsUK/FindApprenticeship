namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    using System.Collections.Generic;

    public class PostalAddressOnlyComparer : IEqualityComparer<PostalAddress>
    {
        public bool Equals(PostalAddress x, PostalAddress y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(x, y)) return true;
            return string.Equals(x.AddressLine1, y.AddressLine1) && string.Equals(x.AddressLine2, y.AddressLine2) && string.Equals(x.AddressLine3, y.AddressLine3) && string.Equals(x.AddressLine4, y.AddressLine4) && string.Equals(x.AddressLine5, y.AddressLine5) && string.Equals(x.County, y.County) && x.CountyId == y.CountyId && Equals(x.GeoPoint, y.GeoPoint) && string.Equals(x.LocalAuthority, y.LocalAuthority) && string.Equals(x.LocalAuthorityCodeName, y.LocalAuthorityCodeName) && x.LocalAuthorityId == y.LocalAuthorityId && string.Equals(x.Postcode, y.Postcode) && string.Equals(x.Town, y.Town);
        }

        public int GetHashCode(PostalAddress obj)
        {
            unchecked
            {
                var hashCode = (obj.AddressLine1 != null ? obj.AddressLine1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AddressLine2 != null ? obj.AddressLine2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AddressLine3 != null ? obj.AddressLine3.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AddressLine4 != null ? obj.AddressLine4.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.AddressLine5 != null ? obj.AddressLine5.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.County != null ? obj.County.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.CountyId;
                hashCode = (hashCode * 397) ^ (obj.GeoPoint != null ? obj.GeoPoint.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.LocalAuthority != null ? obj.LocalAuthority.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.LocalAuthorityCodeName != null ? obj.LocalAuthorityCodeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.LocalAuthorityId;
                hashCode = (hashCode * 397) ^ (obj.Postcode != null ? obj.Postcode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Town != null ? obj.Town.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}