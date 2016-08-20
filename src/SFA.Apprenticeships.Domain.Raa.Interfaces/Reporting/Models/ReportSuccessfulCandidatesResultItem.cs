namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    using System;

    public class ReportSuccessfulCandidatesResultItem
    {
        public int CandidateId { get; set; }
        public Guid CandidateGuid { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Postcode { get; set; }
        public string LearningProvider { get; set; }
        public string VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyType { get; set; }
        public string Sector { get; set; }
        public string Framework { get; set; }
        public string FrameworkStatus { get; set; }
        public string Employer { get; set; }
        public string SuccessfulAppDate { get; set; }
        public string ILRStartDate { get; set; }
        public string ILRReference { get; set; }
    }
}
