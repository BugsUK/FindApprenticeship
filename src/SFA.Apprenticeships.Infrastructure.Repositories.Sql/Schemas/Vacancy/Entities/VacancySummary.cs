namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancySummary
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

        public int? NewApplicantCount { get; set; }
        public int? ApplicantCount { get; set; }

        public string ProviderTradingName { get; set; }

        public DateTime DateSubmitted { get; set; }
        public DateTime DateFirstSubmitted { get; set; }
        public DateTime CreatedDate { get; set; }

        public int SubmissionCount { get; set; }
    }
}