namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSiteLocalAuthority")]
    public partial class ProviderSiteLocalAuthority
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProviderSiteLocalAuthority()
        {
            ProviderSiteOffers = new HashSet<ProviderSiteOffer>();
        }

        public int ProviderSiteLocalAuthorityID { get; set; }

        public int ProviderSiteRelationshipID { get; set; }

        public int? LocalAuthorityId { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        public virtual ProviderSiteRelationship ProviderSiteRelationship { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteOffer> ProviderSiteOffers { get; set; }
    }
}
