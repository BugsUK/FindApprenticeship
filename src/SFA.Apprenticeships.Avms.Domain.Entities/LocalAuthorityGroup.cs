namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocalAuthorityGroup")]
    public partial class LocalAuthorityGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalAuthorityGroup()
        {
            LocalAuthorityGroup1 = new HashSet<LocalAuthorityGroup>();
            LocalAuthorities = new HashSet<LocalAuthority>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocalAuthorityGroupID { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(50)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public int LocalAuthorityGroupTypeID { get; set; }

        public int LocalAuthorityGroupPurposeID { get; set; }

        public int? ParentLocalAuthorityGroupID { get; set; }

        public virtual LocalAuthorityGroupPurpose LocalAuthorityGroupPurpose { get; set; }

        public virtual LocalAuthorityGroupType LocalAuthorityGroupType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalAuthorityGroup> LocalAuthorityGroup1 { get; set; }

        public virtual LocalAuthorityGroup LocalAuthorityGroup2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalAuthority> LocalAuthorities { get; set; }
    }
}
