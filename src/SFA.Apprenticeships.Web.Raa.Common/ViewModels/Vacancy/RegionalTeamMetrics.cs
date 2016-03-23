namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Entities.Raa.Reference;

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