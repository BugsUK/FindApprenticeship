namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting.Models
{
    public class ReportVacanciesResultItem
    {
        public string vacancyid { get; set; }
        public string VacancyTitle { get; set; }
        public string VacancyType { get; set; }
        public string Reference { get; set; }
        public string EmployerName { get; set; }
        public string EmployerNameActual { get; set; }
        public string EmployerAnonymousName { get; set; }
        public string IsEmployerAnonymous { get; set; }
        public string Postcode { get; set; }
        public string Sector { get; set; }
        public string Framework { get; set; }
        public string FrameworkStatus { get; set; }
        public string LearningProvider { get; set; }
        public string NumberOfPositions { get; set; }
        public string DatePosted { get; set; }
        public string ClosingDate { get; set; }
        public string NoOfPositionsAvailable { get; set; }
        public string NoOfApplications { get; set; }
        public string Status { get; set; }
        public string DeliverySite { get; set; }
    }
}
