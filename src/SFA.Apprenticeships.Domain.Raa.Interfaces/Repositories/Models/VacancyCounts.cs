namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    public class VacancyCounts
    {
        public int LiveCount { get; set; }
        public int SubmittedCount { get; set; }
        public int RejectedCount { get; set; }
        public int ClosingSoonCount { get; set; }
        public int ClosedCount { get; set; }
        public int DraftCount { get; set; }
        public int NewApplicationsCount { get; set; }
        public int CompletedCount { get; set; }
    }
}