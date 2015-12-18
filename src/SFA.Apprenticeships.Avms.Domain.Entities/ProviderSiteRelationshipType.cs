namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSiteRelationshipType")]
    public partial class ProviderSiteRelationshipType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProviderSiteRelationshipType()
        {
            ProviderSiteRelationships = new HashSet<ProviderSiteRelationship>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProviderSiteRelationshipTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProviderSiteRelationshipTypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteRelationship> ProviderSiteRelationships { get; set; }
    }
}
