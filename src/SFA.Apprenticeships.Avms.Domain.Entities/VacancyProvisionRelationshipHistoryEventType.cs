namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyProvisionRelationshipHistoryEventType")]
    public partial class VacancyProvisionRelationshipHistoryEventType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyProvisionRelationshipHistoryEventType()
        {
            VacancyOwnerRelationshipHistories = new HashSet<VacancyOwnerRelationshipHistory>();
        }

        public int VacancyProvisionRelationshipHistoryEventTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationshipHistory> VacancyOwnerRelationshipHistories { get; set; }
    }
}
