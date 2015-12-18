namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSiteRelationship")]
    public partial class ProviderSiteRelationship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProviderSiteRelationship()
        {
            ProviderSiteFrameworks = new HashSet<ProviderSiteFramework>();
            ProviderSiteLocalAuthorities = new HashSet<ProviderSiteLocalAuthority>();
            VacancyOwnerRelationships = new HashSet<VacancyOwnerRelationship>();
        }

        public int ProviderSiteRelationshipID { get; set; }

        public int ProviderID { get; set; }

        public int ProviderSiteID { get; set; }

        public int ProviderSiteRelationShipTypeID { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual ProviderSite ProviderSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteFramework> ProviderSiteFrameworks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteLocalAuthority> ProviderSiteLocalAuthorities { get; set; }

        public virtual ProviderSiteRelationshipType ProviderSiteRelationshipType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationship> VacancyOwnerRelationships { get; set; }
    }
}
