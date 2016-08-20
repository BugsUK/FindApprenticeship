namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    using System;

    public class ReportUnsuccessfulCandidatesResultItem
    {
        public int CandidateId { get; set; }
        public Guid CandidateGuid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public string Disability { get; set; }
        public string AllowMarketingMessages { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string AuthorityArea { get; set; }
        public string ShortAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Unsuccessful { get; set; }
        public string Ongoing { get; set; }
        public string Withdrawn { get; set; }
        public string DateApplied { get; set; }
        public string VacancyClosingDate { get; set; }
        public string DateOfUnsuccessfulNotification { get; set; }
        public string LearningProvider { get; set; }
        public string LearningProviderUKPRN { get; set; }
        public string VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyLevel { get; set; }
        public string Sector { get; set; }
        public string Framework { get; set; }
        public string UnsuccessfulReason { get; set; }
        public string Notes { get; set; }
        public string Points { get; set; }
    }
}
