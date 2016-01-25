namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Organisation")]
    public partial class Organisation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organisation()
        {
            StakeHolders = new HashSet<StakeHolder>();
        }

        public int OrganisationId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StakeHolder> StakeHolders { get; set; }
    }
}
