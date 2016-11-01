namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using TrainingType = Domain.Entities.Raa.Vacancies.TrainingType;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancySummary : IVacancyWage
    {
        // mild hack to fetch the total amount of records returned in the query
        public int TotalResultCount { get; set; }


        public int VacancyId { get; set; }
        public int VacancyOwnerRelationshipId { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public Guid VacancyGuid { get; set; }
        public string Title { get; set; }
        public bool? ApplyOutsideNAVMS { get; set; }
        public int NoOfOfflineApplicants { get; set; }
        public VacancyStatus VacancyStatusId { get; set; }
        public VacancyType VacancyTypeId { get; set; }
        
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public int CountyId { get; set; }
        public string PostCode { get; set; }

        public DateTime? ApplicationClosingDate { get; set; }
        
        public string EmployerName { get; set; }
        public string EmployerLocation { get; set; }

        public int? NewApplicantCount { get; set; }
        public int? ApplicantCount { get; set; }

        public string ProviderTradingName { get; set; }

        public DateTime DateSubmitted { get; set; }
        public DateTime DateFirstSubmitted { get; set; }
        public DateTime CreatedDate { get; set; }

        public int SubmissionCount { get; set; }

        public DateTime? StartedToQADateTime { get; set; }

        public string QAUserName { get; set; }
		
		public string FrameworkCodeName { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string SectorCodeName { get; set; }

        public int EmployerId { get; set; }
        public int ContractOwnerId { get; set; }
        public DateTime PossibleStartDate { get; set; }
        public int NumberOfPositions { get; set; }
        public DateTime DateQAApproved { get; set; }

        public string WageText { get; set; }

        public int WageType { get; set; }

        public int? WageUnitId { get; set; }

        public decimal? WeeklyWage { get; set; }
        public decimal? HoursPerWeek { get; set; }

        public string ShortDescription { get; set; }
        public int? VacancyLocationTypeId { get; set; }
        public int? StandardId { get; set; }
        public int? DeliveryOrganisationId { get; set; }
        public int? Duration { get; set; }
        public DurationType DurationType { get; set; }
        public string EmployerAnonymousName { get; set; }
        public string ExpectedDuration { get; set; }
        public int? OriginalContractOwnerId { get; set; }
        public RegionalTeam RegionalTeam { get; set; }
        public TrainingType TrainingTypeId { get; set; }
        public int? VacancyManagerId { get; set; }
        public string WorkingWeek { get; set; }
        public int? ParentVacancyId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? GeocodeEasting { get; set; }
        public int? GeocodeNorthing { get; set; }
    }
}