namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("School")]
    public partial class School
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public School()
        {
            SchoolAttendeds = new HashSet<SchoolAttended>();
        }

        public int SchoolId { get; set; }

        [Required]
        [StringLength(100)]
        public string URN { get; set; }

        [Required]
        [StringLength(120)]
        public string SchoolName { get; set; }

        [Required]
        [StringLength(2000)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string Area { get; set; }

        [StringLength(100)]
        public string Town { get; set; }

        [StringLength(100)]
        public string County { get; set; }

        [StringLength(10)]
        public string Postcode { get; set; }

        [StringLength(120)]
        public string SchoolNameForSearch { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SchoolAttended> SchoolAttendeds { get; set; }
    }
}
