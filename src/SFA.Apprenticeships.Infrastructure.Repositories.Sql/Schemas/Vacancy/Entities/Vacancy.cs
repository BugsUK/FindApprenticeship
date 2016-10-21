namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Vacancy")]
    public class Vacancy
    {
        public string AdditionalLocationInformation { get; set; }
        public string AdditionalLocationInformationComment { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ApplicationClosingDate { get; set; }

        public bool? ApplyOutsideNAVMS { get; set; }

        public int? ApprenticeshipFrameworkId { get; set; }
        public string ApprenticeshipLevelComment { get; set; }

        public int? ApprenticeshipType { get; set; }

        public string BeingSupportedBy { get; set; }
        public string ClosingDateComment { get; set; }
        public string ContactDetailsComment { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public int? ContractOwnerID { get; set; }

        public int CountyId { get; set; }

        public int? DeliveryOrganisationID { get; set; }

        public string Description { get; set; }
        public string DesiredQualifications { get; set; }
        public string DesiredQualificationsComment { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredSkillsComment { get; set; }
        public string DurationComment { get; set; }

        public int DurationTypeId { get; set; }

        public int? DurationValue { get; set; }

        public bool EditedInRaa { get; set; }

        public string EmployerAnonymousName { get; set; }

        public string EmployerDescription { get; set; }
        public string EmployerDescriptionComment { get; set; }

        public string EmployersApplicationInstructions { get; set; }

        public string EmployersRecruitmentWebsite { get; set; }

        public string EmployersWebsite { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }

        public string ExpectedDuration { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ExpectedStartDate { get; set; }


        public string FirstQuestion { get; set; }
        public string FirstQuestionComment { get; set; }
        public string FrameworkCodeNameComment { get; set; }
        public string FutureProspects { get; set; }
        public string FutureProspectsComment { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? HoursPerWeek { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InterviewsFromDate { get; set; }

        public decimal? Latitude { get; set; }

        public int? LocalAuthorityId { get; set; }
        public string LocationAddressesComment { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }
        public string LongDescriptionComment { get; set; }

        public decimal? Longitude { get; set; }

        public int? MasterVacancyId { get; set; }

        public int? MaxNumberofApplications { get; set; }

        public int? NoOfOfflineApplicants { get; set; }

        public int? NoOfOfflineSystemApplicants { get; set; }

        [Column(TypeName = "smallint")]
        public short? NumberOfPositions { get; set; }

        public string NumberOfPositionsComment { get; set; }

        public int? NumberOfViews { get; set; }
        public string OfflineApplicationInstructionsComment { get; set; }
        public string OfflineApplicationUrlComment { get; set; }

        public int? OfflineVacancyTypeId { get; set; }

        public int? OriginalContractOwnerId { get; set; }
        public string OtherInformation { get; set; }
        public string PersonalQualities { get; set; }
        public string PersonalQualitiesComment { get; set; }
        public string PossibleStartDateComment { get; set; }

        public string PostCode { get; set; }

        public string QAUserName { get; set; }
        public string SecondQuestion { get; set; }
        public string SecondQuestionComment { get; set; }
        public string SectorCodeNameComment { get; set; }

        public int? SectorId { get; set; }

        public string ShortDescription { get; set; }
        public string ShortDescriptionComment { get; set; }

        public bool SmallEmployerWageIncentive { get; set; }

        public int? StandardId { get; set; }
        public string StandardIdComment { get; set; }

        public DateTime? StartedToQADateTime { get; set; }

        public int SubmissionCount { get; set; }
        public string ThingsToConsider { get; set; }
        public string ThingsToConsiderComment { get; set; }

        [Required]
        public string Title { get; set; }

        public string TitleComment { get; set; }

        public string Town { get; set; }
        public string TrainingProvided { get; set; }
        public string TrainingProvidedComment { get; set; }

        public int TrainingTypeId { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public Guid VacancyGuid { get; set; }
        public int VacancyId { get; set; }

        public int? VacancyLocationTypeId { get; set; }

        public bool VacancyManagerAnonymous { get; set; }

        public int? VacancyManagerID { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public int VacancySourceId { get; set; }

        public int VacancyStatusId { get; set; }

        public int VacancyTypeId { get; set; }
        public string WageComment { get; set; }

        public string WageText { get; set; }

        public int WageType { get; set; }

        public int? WageUnitId { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; }

        public string WorkingWeek { get; set; }
        public string WorkingWeekComment { get; set; }
    }
}