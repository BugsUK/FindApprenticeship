namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApprenticeshipOccupation")]
    public partial class ApprenticeshipOccupation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApprenticeshipOccupation()
        {
            ApprenticeshipFrameworks = new HashSet<ApprenticeshipFramework>();
            ApprenticeshipFrameworks1 = new HashSet<ApprenticeshipFramework>();
            CandidatePreferences = new HashSet<CandidatePreference>();
            CandidatePreferences1 = new HashSet<CandidatePreference>();
            SectorSuccessRates = new HashSet<SectorSuccessRate>();
        }

        public int ApprenticeshipOccupationId { get; set; }

        [Required]
        [StringLength(3)]
        public string Codename { get; set; }

        [Required]
        [StringLength(50)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public int ApprenticeshipOccupationStatusTypeId { get; set; }

        public DateTime? ClosedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApprenticeshipFramework> ApprenticeshipFrameworks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApprenticeshipFramework> ApprenticeshipFrameworks1 { get; set; }

        public virtual ApprenticeshipOccupationStatusType ApprenticeshipOccupationStatusType { get; set; }

        public virtual ApprenticeshipOccupationStatusType ApprenticeshipOccupationStatusType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidatePreference> CandidatePreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidatePreference> CandidatePreferences1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SectorSuccessRate> SectorSuccessRates { get; set; }
    }
}
