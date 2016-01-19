namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocalAuthorityGroupPurpose")]
    public partial class LocalAuthorityGroupPurpose
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalAuthorityGroupPurpose()
        {
            LocalAuthorityGroups = new HashSet<LocalAuthorityGroup>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocalAuthorityGroupPurposeID { get; set; }

        [Required]
        [StringLength(50)]
        public string LocalAuthorityGroupPurposeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalAuthorityGroup> LocalAuthorityGroups { get; set; }
    }
}
