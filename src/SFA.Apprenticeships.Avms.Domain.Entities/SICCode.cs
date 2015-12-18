namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SICCode")]
    public partial class SICCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SICCode()
        {
            EmployerSICCodes = new HashSet<EmployerSICCode>();
        }

        public int SICCodeId { get; set; }

        public short Year { get; set; }

        [Column("SICCode")]
        public int SICCode1 { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployerSICCode> EmployerSICCodes { get; set; }
    }
}
