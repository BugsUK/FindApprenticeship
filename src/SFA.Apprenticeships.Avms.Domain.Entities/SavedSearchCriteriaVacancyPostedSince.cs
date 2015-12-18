namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SavedSearchCriteriaVacancyPostedSince")]
    public partial class SavedSearchCriteriaVacancyPostedSince
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SavedSearchCriteriaVacancyPostedSince()
        {
            SavedSearchCriterias = new HashSet<SavedSearchCriteria>();
        }

        [Key]
        [Column("SavedSearchCriteriaVacancyPostedSince")]
        public int SavedSearchCriteriaVacancyPostedSince1 { get; set; }

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
        public virtual ICollection<SavedSearchCriteria> SavedSearchCriterias { get; set; }
    }
}
