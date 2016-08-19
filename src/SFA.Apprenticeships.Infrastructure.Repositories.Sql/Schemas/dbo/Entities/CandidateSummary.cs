namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using System;

    public class CandidateSummary
    {
        public int CandidateId { get; set; }
        public Guid CandidateGuid { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}