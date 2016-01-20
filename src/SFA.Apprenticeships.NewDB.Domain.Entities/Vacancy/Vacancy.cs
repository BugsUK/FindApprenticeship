namespace SFA.Apprenticeships.NewDB.Domain.Entities.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vacancy.Vacancy")]
    public partial class Vacancy
    {
        /*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vacancy()
        {
            Vacancy1 = new HashSet<Vacancy>();
            VacancyLocations = new HashSet<VacancyLocation>();
        }
        */

        public Guid VacancyId { get; set; }

        public int? VacancyReferenceNumber { get; set; }

        [Required]
        [StringLength(1)]
        public string VacancyTypeCode { get; set; }

        [Required]
        [StringLength(3)]
        public string VacancyStatusCode { get; set; }

        [Required]
        [StringLength(1)]
        public string VacancyLocationTypeCode { get; set; }

        public Guid? ParentVacancyId { get; set; }

        public int EmployerVacancyPartyId { get; set; }

        public int OwnerVacancyPartyId { get; set; }

        public int ManagerVacancyPartyId { get; set; }

        public int DeliveryProviderVacancyPartyId { get; set; }

        public int ContractOwnerVacancyPartyId { get; set; }

        public int? OriginalContractOwnerVacancyPartyId { get; set; }

        [Required]
        public string Title { get; set; }

        public string TitleComment { get; set; }

        public string ShortDescription { get; set; }

        public string ShortDescriptionComment { get; set; }

        public string LongDescription { get; set; }

        public string LongDescriptionComment { get; set; }

        [Required]
        [StringLength(1)]
        public string TrainingTypeCode { get; set; }

        public int? FrameworkId { get; set; }

        public string FrameworkIdComment { get; set; }

        public int? StandardId { get; set; }

        public string StandardIdComment { get; set; }

        [Required]
        [StringLength(1)]
        public string LevelCode { get; set; }

        public string LevelCodeComment { get; set; }

        [Column(TypeName = "money")]
        public decimal? WageValue { get; set; }

        [StringLength(3)]
        public string WageTypeCode { get; set; }

        public string WageIntervalCode { get; set; }

        public string AV_WageText { get; set; }

        public string WageComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? PossibleStartDateDate { get; set; }

        public string PossibleStartDateComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClosingDate { get; set; }

        public string ClosingDateComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AV_InterviewStartDate { get; set; }

        public short? DurationValue { get; set; }

        [StringLength(1)]
        public string DurationTypeCode { get; set; }

        public string DurationComment { get; set; }

        public string WorkingWeekText { get; set; }

        public string WorkingWeekComment { get; set; }

        public decimal? HoursPerWeek { get; set; }

        public string AV_ContactName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerDescriptionComment { get; set; }

        public string EmployerWebsiteUrl { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }

        public bool? IsDirectApplication { get; set; }

        public string DirectApplicationUrl { get; set; }

        public string DirectApplicationUrlComment { get; set; }

        public string DirectApplicationInstructions { get; set; }

        public string DirectApplicationInstructionsComment { get; set; }

        public string AdditionalLocationInformation { get; set; }
        public string AdditionalLocationInformationComment { get; set; }

        public string DesiredSkills { get; set; }

        public string DesiredSkillsComment { get; set; }

        public string FutureProspects { get; set; }

        public string FutureProspectsComment { get; set; }

        public string PersonalQualities { get; set; }

        public string PersonalQualitiesComment { get; set; }

        public string ThingsToConsider { get; set; }

        public string ThingsToConsiderComment { get; set; }

        public string DesiredQualifications { get; set; }

        public string DesiredQualificationsComment { get; set; }

        public string FirstQuestion { get; set; }

        public string FirstQuestionComment { get; set; }

        public string SecondQuestion { get; set; }

        public string SecondQuestionComment { get; set; }

        public string QAUserName { get; set; }

        public string LocationAddressesComment { get; set; }

        public int NumberOfPositions { get; set; }

        public string NumberOfPositionsComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateQAApproved { get; set; }

        public DateTime? PublishedDateTime { get; set; } // TODO: Check naming

        public DateTime? FirstSubmittedDateTime { get; set; } // TODO: Check naming

        public int SubmissionCount { get; set; }

        public int OfflineApplicationClickThroughCount { get; set; }

        /*
        public virtual Framework Framework { get; set; }

        public virtual Level Level { get; set; }

        public virtual Standard Standard { get; set; }

        public virtual DurationType DurationType { get; set; }

        public virtual TrainingType TrainingType { get; set; }

        public virtual VacancyParty VacancyParty { get; set; }

        public virtual VacancyParty VacancyParty1 { get; set; }

        public virtual VacancyParty VacancyParty2 { get; set; }

        public virtual VacancyParty VacancyParty3 { get; set; }

        public virtual VacancyParty VacancyParty4 { get; set; }

        public virtual VacancyParty VacancyParty5 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancy1 { get; set; }

        public virtual Vacancy Vacancy2 { get; set; }

        public virtual VacancyLocationType VacancyLocationType { get; set; }

        public virtual VacancyStatu VacancyStatu { get; set; }

        public virtual VacancyType VacancyType { get; set; }

        public virtual WageType WageType { get; set; }
        */
    }
}
