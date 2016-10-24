namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Domain.Entities.Raa.Vacancies;

    [Table("dbo.Vacancy")]
    public class Vacancy
    {
        public int VacancyId { get; set; }

        public Guid VacancyGuid { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        public int VacancyTypeId { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public string ContactName { get; set; }

        public int VacancyStatusId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public int CountyId { get; set; }

        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? ApprenticeshipFrameworkId { get; set; }

        public int? StandardId { get; set; }

        [Required]
        public string Title { get; set; }

        public int? ApprenticeshipType { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; }

        public int WageType { get; set; }

        public string WageText { get; set; }

        public int? WageUnitId { get; set; }

        [Column(TypeName = "smallint")]
        public short? NumberOfPositions { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ApplicationClosingDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InterviewsFromDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ExpectedStartDate { get; set; }

        public int? DurationValue { get; set; }

        public string ExpectedDuration { get; set; }

        public int DurationTypeId { get; set; }

        public string WorkingWeek { get; set; }

        public int? NumberOfViews { get; set; }

        public string EmployerAnonymousName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployersWebsite { get; set; }

        public int? MaxNumberofApplications { get; set; }

        public bool? ApplyOutsideNAVMS { get; set; }

        public string EmployersApplicationInstructions { get; set; }

        public string EmployersRecruitmentWebsite { get; set; }

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

        public int TrainingTypeId { get; set; }

        public decimal? HoursPerWeek { get; set; }

        public string AdditionalLocationInformation { get; set; }

        public string QAUserName { get; set; }

        public int SubmissionCount { get; set; }

        public DateTime? StartedToQADateTime { get; set; }

        public string ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        public bool EditedInRaa { get; set; }

        public int? SectorId { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int VacancySourceId { get; set; }

        public int? OfflineVacancyTypeId { get; set; }
    }
}
