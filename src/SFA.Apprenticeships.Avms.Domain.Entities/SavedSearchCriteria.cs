namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SavedSearchCriteria")]
    public partial class SavedSearchCriteria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SavedSearchCriteria()
        {
            SearchFrameworks = new HashSet<SearchFramework>();
        }

        public int SavedSearchCriteriaId { get; set; }

        public int CandidateId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int SearchType { get; set; }

        public int? CountyId { get; set; }

        [StringLength(8)]
        public string Postcode { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public short? DistanceFromPostcode { get; set; }

        public short? MinWages { get; set; }

        public short? MaxWages { get; set; }

        public int? VacancyReferenceNumber { get; set; }

        [StringLength(255)]
        public string Employer { get; set; }

        [StringLength(255)]
        public string TrainingProvider { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        public DateTime? DateSearched { get; set; }

        public bool BackgroundSearch { get; set; }

        public bool AlertSent { get; set; }

        public int? CountBackgroundMatches { get; set; }

        public int VacancyPostedSince { get; set; }

        public int? ApprenticeshipTypeId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual SavedSearchCriteriaSearchType SavedSearchCriteriaSearchType { get; set; }

        public virtual SavedSearchCriteriaVacancyPostedSince SavedSearchCriteriaVacancyPostedSince { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchFramework> SearchFrameworks { get; set; }
    }
}
