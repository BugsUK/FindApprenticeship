namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    public class ReportVacancyExtensionsResultItem
    {
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public string VacancyReferenceNumber { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyStatus { get; set; }
        public string OriginalPostingDate { get; set; }
        public string OriginalClosingDate { get; set; }
        public string CurrentClosingDate { get; set; }
        public string NumberOfVacancyExtensions { get; set; }
        public string NumberOfSubmittedApplications { get; set; }
    }
}