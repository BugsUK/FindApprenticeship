namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using System;

    public class Employer
    {
        public int EmployerId { get; set; }
        public int EdsUrn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public string AddressLine1 { get; set; }
	    public string AddressLine2 { get; set; }
	    public string AddressLine3 { get; set; }
	    public string AddressLine4 { get; set; }
	    public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public int CountyId { get; set; }
        public string PostCode { get; set; }
        public int? LocalAuthorityId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int? GeocodeEasting { get; set; }
        public int? GeocodeNorthing { get; set; }
        public int PrimaryContact { get; set; }
        public int? NumberofEmployeesAtSite { get; set; }
        public int? NumberOfEmployeesInGroup { get; set; }
        public string OwnerOrgnistaion { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public int? TotalVacanciesPosted { get; set; }
        public string BeingSupportedBy { get; set; }
        public DateTime? LockedForSupportUntil { get; set; }
        public int? EmployerStatusTypeId { get; set; }
        public bool DisableAllowed { get; set; }
        public bool TrackingAllowed { get; set; }
    }
}