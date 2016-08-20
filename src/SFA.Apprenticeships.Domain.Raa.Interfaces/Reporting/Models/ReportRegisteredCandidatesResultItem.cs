namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    using System;

    public class ReportRegisteredCandidatesResultItem
    {
        public int CandidateId { get; set; }
        public Guid CandidateGuid { get; set; }
        public string Name { get; set; }
        public string DateofBirth { get; set; }
        public string Region { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string ShortAddress { get; set; }
        public string LastSchool { get; set; }
        public string DateLastActive { get; set; }
        public string RegistrationDate { get; set; }
        public string Address { get; set; }
        public string LandlineNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int ApplicationCount { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Ethnicity { get; set; }
        public bool Inactive { get; set; }
        public string Sector { get; set; }
        public string Framework { get; set; }
        public string Keyword { get; set; }
        public string CandidateStatus { get; set; }
        public bool DregisteredCandidate { get; set; }
        public string AllowMarketingMessages { get; set; }
    }
}
