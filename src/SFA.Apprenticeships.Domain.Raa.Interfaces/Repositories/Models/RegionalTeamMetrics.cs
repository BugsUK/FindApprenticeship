namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    using Entities.Raa.Reference;

    public class RegionalTeamMetrics
    {
        public RegionalTeam RegionalTeam { get; set; }
        public int TotalCount { get; set; }
        public int SubmittedTodayCount { get; set; }
        public int SubmittedYesterdayCount { get; set; }
        public int SubmittedMoreThan48HoursCount { get; set; }
        public int ResubmittedCount { get; set; }
    }
}