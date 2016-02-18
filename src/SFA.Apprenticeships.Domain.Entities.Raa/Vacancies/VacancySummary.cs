namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Locations;

    public class VacancySummary
    {
        public int VacancyId { get; set; }
        public VacancyType VacancyType { get; set; }
        public VacancyStatus Status { get; set; }
        public string Title { get; set; }
        public string ProviderName { get; set; }
        public string EmployerName { get; set; }
        public PostalAddress Location { get; set; }
        public bool OfflineVacancy { get; set; }
        public int ApplicationOrClickThroughCount { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public int SubmissionCount { get; set; }
    }
}