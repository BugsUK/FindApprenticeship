namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApprenticeshipFramework")]
    public partial class ApprenticeshipFramework
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApprenticeshipFramework()
        {
            CandidatePreferences = new HashSet<CandidatePreference>();
            CandidatePreferences1 = new HashSet<CandidatePreference>();
            SearchFrameworks = new HashSet<SearchFramework>();
            ProviderSiteFrameworks = new HashSet<ProviderSiteFramework>();
            Vacancies = new HashSet<Vacancy>();
            VacancySearches = new HashSet<VacancySearch>();
        }

        public int ApprenticeshipFrameworkId { get; set; }

        public int ApprenticeshipOccupationId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        public int ApprenticeshipFrameworkStatusTypeId { get; set; }

        public DateTime? ClosedDate { get; set; }

        public int? PreviousApprenticeshipOccupationId { get; set; }

        public virtual ApprenticeshipFrameworkStatusType ApprenticeshipFrameworkStatusType { get; set; }

        public virtual ApprenticeshipOccupation ApprenticeshipOccupation { get; set; }

        public virtual ApprenticeshipOccupation ApprenticeshipOccupation1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidatePreference> CandidatePreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidatePreference> CandidatePreferences1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchFramework> SearchFrameworks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteFramework> ProviderSiteFrameworks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancySearch> VacancySearches { get; set; }
    }
}
