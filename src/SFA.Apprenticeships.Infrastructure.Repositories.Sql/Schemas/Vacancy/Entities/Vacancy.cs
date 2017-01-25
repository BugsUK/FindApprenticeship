namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using Dapper.Contrib.Extensions;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [System.ComponentModel.DataAnnotations.Schema.Table("dbo.Vacancy")]
    public class Vacancy : IVacancyWage
    {
        [Write(false)]
        public DateTime? DateQAApproved { get; set; }

        [Write(false)]
        public DateTime? DateFirstSubmitted { get; set; }

        [Write(false)]
        public DateTime? DateSubmitted { get; set; }

        public string AdditionalLocationInformation { get; set; }

        [Write(false)]
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
        [Write(false)]
        public string ApprenticeshipLevelComment { get; set; }

        public int? ApprenticeshipType { get; set; }

        public string BeingSupportedBy { get; set; }

        [Write(false)]
        public string ClosingDateComment { get; set; }

        [Write(false)]
        public string ContactDetailsComment { get; set; }

        public string ContactEmail { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public int? ContractOwnerID { get; set; }

        public int CountyId { get; set; }

        public int? DeliveryOrganisationID { get; set; }

        public string Description { get; set; }

        [Write(false)]
        public string DesiredQualifications { get; set; }
        [Write(false)]
        public string DesiredQualificationsComment { get; set; }

        [Write(false)]
        public string DesiredSkills { get; set; }
        [Write(false)]
        public string DesiredSkillsComment { get; set; }

        [Write(false)]
        public string DurationComment { get; set; }

        public int DurationTypeId { get; set; }

        public int? DurationValue { get; set; }

        public bool EditedInRaa { get; set; }

        public string EmployerAnonymousName { get; set; }
        public string EmployerAnonymousReason { get; set; }

        public string AnonymousAboutTheEmployer { get; set; }

        [Write(false)]
        public string AnonymousEmployerDescriptionComment { get; set; }
        [Write(false)]
        public string AnonymousEmployerReasonComment { get; set; }

        [Write(false)]
        public string AnonymousAboutTheEmployerComment { get; set; }
        public string EmployerDescription { get; set; }
        [Write(false)]
        public string EmployerDescriptionComment { get; set; }

        public string EmployersApplicationInstructions { get; set; }

        public string EmployersRecruitmentWebsite { get; set; }

        public string EmployersWebsite { get; set; }
        [Write(false)]
        public string EmployerWebsiteUrlComment { get; set; }

        public string ExpectedDuration { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ExpectedStartDate { get; set; }

        [Write(false)]
        public string FirstQuestion { get; set; }
        [Write(false)]
        public string FirstQuestionComment { get; set; }

        [Write(false)]
        public string FrameworkCodeNameComment { get; set; }

        [Write(false)]
        public string FutureProspects { get; set; }
        [Write(false)]
        public string FutureProspectsComment { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? HoursPerWeek { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InterviewsFromDate { get; set; }

        public decimal? Latitude { get; set; }

        public int? LocalAuthorityId { get; set; }
        [Write(false)]
        public string LocationAddressesComment { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }
        [Write(false)]
        public string LongDescriptionComment { get; set; }

        public decimal? Longitude { get; set; }

        public int? MasterVacancyId { get; set; }

        public int? MaxNumberofApplications { get; set; }

        public int? NoOfOfflineApplicants { get; set; }

        public int? NoOfOfflineSystemApplicants { get; set; }

        [Column(TypeName = "smallint")]
        public short? NumberOfPositions { get; set; }

        [Write(false)]
        public string NumberOfPositionsComment { get; set; }

        public int? NumberOfViews { get; set; }

        [Write(false)]
        public string OfflineApplicationInstructionsComment { get; set; }

        [Write(false)]
        public string OfflineApplicationUrlComment { get; set; }

        public int? OfflineVacancyTypeId { get; set; }

        public int? OriginalContractOwnerId { get; set; }

        [Write(false)]
        public string OtherInformation { get; set; }

        [Write(false)]
        public string OtherInformationComment { get; set; }

        [Write(false)]
        public string PersonalQualities { get; set; }

        [Write(false)]
        public string PersonalQualitiesComment { get; set; }
        [Write(false)]
        public string PossibleStartDateComment { get; set; }

        public string PostCode { get; set; }

        public string QAUserName { get; set; }

        [Write(false)]
        public string SecondQuestion { get; set; }

        [Write(false)]
        public string SecondQuestionComment { get; set; }
        [Write(false)]
        public string SectorCodeNameComment { get; set; }

        public int? SectorId { get; set; }

        public string ShortDescription { get; set; }
        [Write(false)]
        public string ShortDescriptionComment { get; set; }

        public bool SmallEmployerWageIncentive { get; set; }

        public int? StandardId { get; set; }
        [Write(false)]
        public string StandardIdComment { get; set; }

        public DateTime? StartedToQADateTime { get; set; }

        public int SubmissionCount { get; set; }

        [Write(false)]
        public string ThingsToConsider { get; set; }
        [Write(false)]
        public string ThingsToConsiderComment { get; set; }

        [Required]
        public string Title { get; set; }

        [Write(false)]
        public string TitleComment { get; set; }

        public string Town { get; set; }

        [Write(false)]
        public string TrainingProvided { get; set; }
        [Write(false)]
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

        [Write(false)]
        public string WageComment { get; set; }

        [Column(TypeName = "money")]
        public decimal? WageLowerBound { get; set; }

        [Column(TypeName = "money")]
        public decimal? WageUpperBound { get; set; }

        public string WageText { get; set; }

        public int WageType { get; set; }

        public string WageTypeReason { get; set; }

        public int? WageUnitId { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; }

        public string WorkingWeek { get; set; }

        [Write(false)]
        public string WorkingWeekComment { get; set; }

        [Write(false)]
        public string FrameworkCodeName { get; set; }

        [Write(false)]
        public string SectorCodeName { get; set; }

        [Write(false)]
        public RegionalTeam RegionalTeam { get; set; }

        [Write(false)]
        public string EmployerName { get; set; }

        [Write(false)]
        public string EmployerLocation { get; set; }

        [Write(false)]
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        [Write(false)]
        public int? NewApplicantCount { get; set; }

        [Write(false)]
        public int? ApplicantCount { get; set; }

        [Write(false)]
        public string CreatedByProviderUsername { get; set; }

        [Write(false)]
        public DateTime CreatedDate { get; set; }

        [Write(false)]
        public int EmployerId { get; set; }

        [Write(false)]
        public string LocalAuthorityCode { get; set; }

        [Write(false)]
        public bool? IsMultiLocation { get; set; }

        [Write(false)]
        public FrameworkStatusType? FrameworkStatus { get; set; }

        [Write(false)]
        public FrameworkStatusType? StandardStatus { get; set; }
    }
}