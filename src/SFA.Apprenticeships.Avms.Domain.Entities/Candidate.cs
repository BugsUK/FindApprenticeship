namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Candidate")]
    public partial class Candidate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidate()
        {
            AlertPreferences = new HashSet<AlertPreference>();
            Applications = new HashSet<Application>();
            CAFFields = new HashSet<CAFField>();
            CandidateHistories = new HashSet<CandidateHistory>();
            CandidatePreferences = new HashSet<CandidatePreference>();
            EducationResults = new HashSet<EducationResult>();
            ExternalApplications = new HashSet<ExternalApplication>();
            SavedSearchCriterias = new HashSet<SavedSearchCriteria>();
            SchoolAttendeds = new HashSet<SchoolAttended>();
            WatchedVacancies = new HashSet<WatchedVacancy>();
            WorkExperiences = new HashSet<WorkExperience>();
            Messages = new HashSet<Message>();
        }

        public int CandidateId { get; set; }

        public int PersonId { get; set; }

        public int CandidateStatusTypeId { get; set; }

        public DateTime DateofBirth { get; set; }

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
        public string Postcode { get; set; }

        public int LocalAuthorityId { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        [StringLength(10)]
        public string NiReference { get; set; }

        public int? VoucherReferenceNumber { get; set; }

        public long? UniqueLearnerNumber { get; set; }

        public int UlnStatusId { get; set; }

        public int Gender { get; set; }

        public int EthnicOrigin { get; set; }

        [StringLength(50)]
        public string EthnicOriginOther { get; set; }

        public bool ApplicationLimitEnforced { get; set; }

        public DateTime? LastAccessedDate { get; set; }

        [StringLength(50)]
        public string AdditionalEmail { get; set; }

        public int Disability { get; set; }

        [StringLength(256)]
        public string DisabilityOther { get; set; }

        [StringLength(256)]
        public string HealthProblems { get; set; }

        public bool ReceivePushedContent { get; set; }

        public bool ReferralAgent { get; set; }

        public bool DisableAlerts { get; set; }

        [StringLength(100)]
        public string UnconfirmedEmailAddress { get; set; }

        public bool MobileNumberUnconfirmed { get; set; }

        public short? DoBFailureCount { get; set; }

        public bool ForgottenUsernameRequested { get; set; }

        public bool ForgottenPasswordRequested { get; set; }

        public short TextFailureCount { get; set; }

        public short EmailFailureCount { get; set; }

        public DateTime? LastAccessedManageApplications { get; set; }

        public short ReferralPoints { get; set; }

        [StringLength(50)]
        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public bool? NewVacancyAlertEmail { get; set; }

        public bool? NewVacancyAlertSMS { get; set; }

        public bool? AllowMarketingMessages { get; set; }

        public bool ReminderMessageSent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertPreference> AlertPreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Applications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAFField> CAFFields { get; set; }

        public virtual CandidateDisability CandidateDisability { get; set; }

        public virtual CandidateEthnicOrigin CandidateEthnicOrigin { get; set; }

        public virtual CandidateGender CandidateGender { get; set; }

        public virtual CandidateStatu CandidateStatu { get; set; }

        public virtual CandidateULNStatu CandidateULNStatu { get; set; }

        public virtual County County { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        public virtual Person Person { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidateHistory> CandidateHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CandidatePreference> CandidatePreferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationResult> EducationResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalApplication> ExternalApplications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SavedSearchCriteria> SavedSearchCriterias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SchoolAttended> SchoolAttendeds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchedVacancy> WatchedVacancies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
