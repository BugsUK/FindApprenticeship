namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    public class ReportVacancyTrackerResultItem
    {
        public string QAUserName { get; set; }
        public string Reference { get; set; }
        public string ProviderName { get; set; }
        public string DateSubmitted { get; set; }
        public string Outcome { get; set; }
        public string OutComeDate { get; set; }
    }
}
