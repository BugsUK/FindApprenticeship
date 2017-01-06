namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    using System;

    /// <summary>
    /// The rationale behind the creation of this class is that we will eventually move from the existing Address entity,
    /// to this entity, throughout the entirety of the solution (RAA &amp; FAA).
    /// TODO: Remove the existing Address entity, in favour of using this one.  This should be carried out after the DB migration
    /// and private Beta release
    /// </summary>
    public class PostalAddress
    {
        public int PostalAddressId { get; set; }

#region SFA Data Standard compliance minimum field set
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        /// <summary>
        /// AKA ValidationSourceKeyName.
        /// As at 13/01/2015, the SFA Data Standard for Postal Addresses list valid validating bodies as:
        /// 1. Royal Mail PAF file
        /// 2. GeoPlace data
        /// 3. PCA product (uses a PAF file source)
        /// </summary>
        public string ValidationSourceCode { get; set; }
        /// <summary>
        /// For PAF, this is the Unique Delivery Point Reference Number (UDPRN)
        /// For GeoPlace, this is the Unique Property ReferenceNumber (UPRN)
        /// PostCode anywhere uses PAF data, so this is the UDPRN.
        /// </summary>
        public string ValidationSourceKeyValue { get; set; }
#endregion

        public DateTime DateValidated { get; set; }
        public int CountyId { get; set; }
        public int CountyCodeName { get; set; }
        public string County { get; set; }
        public int LocalAuthorityId { get; set; }
        public int LocalAuthorityCodeName { get; set; }
        public string LocalAuthority { get; set; }

        public GeoPoint GeoPoint { get; set; }

        public override string ToString()
        {
            return AddressLine4 ?? AddressLine3 ?? AddressLine2 ?? AddressLine1 ?? Postcode;
        }

        protected bool Equals(PostalAddress other)
        {
            return string.Equals(AddressLine1, other.AddressLine1) && string.Equals(AddressLine2, other.AddressLine2) && string.Equals(AddressLine3, other.AddressLine3) && string.Equals(AddressLine4, other.AddressLine4) && string.Equals(AddressLine5, other.AddressLine5) && string.Equals(County, other.County) && DateValidated.Equals(other.DateValidated) && Equals(GeoPoint, other.GeoPoint) && PostalAddressId == other.PostalAddressId && string.Equals(Postcode, other.Postcode) && string.Equals(Town, other.Town) && string.Equals(ValidationSourceCode, other.ValidationSourceCode) && string.Equals(ValidationSourceKeyValue, other.ValidationSourceKeyValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PostalAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (AddressLine1 != null ? AddressLine1.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine2 != null ? AddressLine2.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine3 != null ? AddressLine3.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine4 != null ? AddressLine4.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine5 != null ? AddressLine5.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (County != null ? County.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DateValidated.GetHashCode();
                hashCode = (hashCode*397) ^ (GeoPoint != null ? GeoPoint.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ PostalAddressId;
                hashCode = (hashCode*397) ^ (Postcode != null ? Postcode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Town != null ? Town.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ValidationSourceCode != null ? ValidationSourceCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ValidationSourceKeyValue != null ? ValidationSourceKeyValue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
