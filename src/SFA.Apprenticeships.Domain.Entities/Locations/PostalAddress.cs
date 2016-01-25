using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Domain.Entities.Locations
{
    /// <summary>
    /// The rationale behind the creation of this class is that we will eventually move from the existing Address entity,
    /// to this entity, throughout the entirety of the solution (RAA & FAA).
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
        public string County { get; set; }
        public GeoPoint GeoPoint { get; set; }

        //TODO: Implementing the following two derived fields:
        //public int Easting { get; set; }
        //public int Northing { get; set; }
    }
}
