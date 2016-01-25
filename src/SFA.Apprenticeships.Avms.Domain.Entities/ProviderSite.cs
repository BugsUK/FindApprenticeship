namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSite")]
    public partial class ProviderSite
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProviderSite()
        {
            ProviderSiteRelationships = new HashSet<ProviderSiteRelationship>();
            ProviderSiteHistories = new HashSet<ProviderSiteHistory>();
            Vacancies = new HashSet<Vacancy>();
            Vacancies1 = new HashSet<Vacancy>();
            VacancyOwnerRelationships = new HashSet<VacancyOwnerRelationship>();
        }

        public int ProviderSiteID { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string TradingName { get; set; }

        public int EDSURN { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string AddressLine3 { get; set; }

        [StringLength(50)]
        public string AddressLine4 { get; set; }

        [StringLength(50)]
        public string AddressLine5 { get; set; }

        [Required]
        [StringLength(50)]
        public string Town { get; set; }

        public int CountyId { get; set; }

        [Required]
        [StringLength(8)]
        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? ManagingAreaID { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        [StringLength(255)]
        public string OwnerOrganisation { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        [StringLength(100)]
        public string WebPage { get; set; }

        public bool OutofDate { get; set; }

        [StringLength(256)]
        public string ContactDetailsForEmployer { get; set; }

        [StringLength(256)]
        public string ContactDetailsForCandidate { get; set; }

        public bool HideFromSearch { get; set; }

        public int? TrainingProviderStatusTypeId { get; set; }

        public bool IsRecruitmentAgency { get; set; }

        [StringLength(1000)]
        public string ContactDetailsAsARecruitmentAgency { get; set; }

        public virtual County County { get; set; }

        public virtual EmployerTrainingProviderStatu EmployerTrainingProviderStatu { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteRelationship> ProviderSiteRelationships { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteHistory> ProviderSiteHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationship> VacancyOwnerRelationships { get; set; }
    }
}
