namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApprenticeshipOccupationStatusType")]
    public partial class ApprenticeshipOccupationStatusType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApprenticeshipOccupationStatusType()
        {
            ApprenticeshipOccupations = new HashSet<ApprenticeshipOccupation>();
            ApprenticeshipOccupations1 = new HashSet<ApprenticeshipOccupation>();
        }

        public int ApprenticeshipOccupationStatusTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApprenticeshipOccupation> ApprenticeshipOccupations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApprenticeshipOccupation> ApprenticeshipOccupations1 { get; set; }
    }
}
