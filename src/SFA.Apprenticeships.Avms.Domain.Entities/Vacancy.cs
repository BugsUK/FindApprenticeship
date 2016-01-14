namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vacancy")]
    public partial class Vacancy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancy()
        {
            AdditionalQuestions = new HashSet<AdditionalQuestion>();
            Applications = new HashSet<Application>();
            ExternalApplications = new HashSet<ExternalApplication>();
            SubVacancies = new HashSet<SubVacancy>();
            Vacancy1 = new HashSet<Vacancy>();
            VacancyHistories = new HashSet<VacancyHistory>();
            VacancyLocations = new HashSet<VacancyLocation>();
            VacancyReferralComments = new HashSet<VacancyReferralComment>();
            VacancySearches = new HashSet<VacancySearch>();
            VacancyTextFields = new HashSet<VacancyTextField>();
            WatchedVacancies = new HashSet<WatchedVacancy>();
        }

        public int VacancyId { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        public int? VacancyReferenceNumber { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        public int VacancyStatusId { get; set; }

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

        [StringLength(40)]
        public string Town { get; set; }

        public int? CountyId { get; set; }

        [StringLength(8)]
        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? ApprenticeshipFrameworkId { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public int? ApprenticeshipType { get; set; }

        [StringLength(256)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; }

        public int WageType { get; set; }

        [StringLength(50)]
        public string WageText { get; set; }

        public short? NumberofPositions { get; set; }

        public DateTime? ApplicationClosingDate { get; set; }

        public DateTime? InterviewsFromDate { get; set; }

        public DateTime? ExpectedStartDate { get; set; }

        [StringLength(50)]
        public string ExpectedDuration { get; set; }

        [StringLength(50)]
        public string WorkingWeek { get; set; }

        public int? NumberOfViews { get; set; }

        [StringLength(255)]
        public string EmployerAnonymousName { get; set; }

        public string EmployerDescription { get; set; }

        [StringLength(256)]
        public string EmployersWebsite { get; set; }

        public int? MaxNumberofApplications { get; set; }

        public bool? ApplyOutsideNAVMS { get; set; }

        public string EmployersApplicationInstructions { get; set; }

        [StringLength(256)]
        public string EmployersRecruitmentWebsite { get; set; }

        [StringLength(50)]
        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public int? NoOfOfflineApplicants { get; set; }

        public int? MasterVacancyId { get; set; }

        public int? VacancyLocationTypeId { get; set; }

        public int? NoOfOfflineSystemApplicants { get; set; }

        public int? VacancyManagerID { get; set; }

        public int? DeliveryOrganisationID { get; set; }

        public int? ContractOwnerID { get; set; }

        public bool SmallEmployerWageIncentive { get; set; }

        public int? OriginalContractOwnerId { get; set; }

        public bool VacancyManagerAnonymous { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdditionalQuestion> AdditionalQuestions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Applications { get; set; }

        public virtual ApprenticeshipFramework ApprenticeshipFramework { get; set; }

        public virtual ApprenticeshipType ApprenticeshipType1 { get; set; }

        public virtual County County { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalApplication> ExternalApplications { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual ProviderSite ProviderSite { get; set; }

        public virtual ProviderSite ProviderSite1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubVacancy> SubVacancies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancy1 { get; set; }

        public virtual Vacancy Vacancy2 { get; set; }

        public virtual VacancyOwnerRelationship VacancyOwnerRelationship { get; set; }

        public virtual VacancyStatusType VacancyStatusType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyHistory> VacancyHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyLocation> VacancyLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyReferralComment> VacancyReferralComments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancySearch> VacancySearches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyTextField> VacancyTextFields { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchedVacancy> WatchedVacancies { get; set; }
    }
}
