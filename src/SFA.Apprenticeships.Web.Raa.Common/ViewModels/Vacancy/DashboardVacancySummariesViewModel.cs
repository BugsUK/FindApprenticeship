namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;

    public class DashboardVacancySummariesViewModel
    {
        public DashboardVacancySummariesSearchViewModel SearchViewModel { get; set; }

        public int SubmittedTodayCount { get; set; }

        public int SubmittedYesterdayCount { get; set; }

        public int SubmittedMoreThan48HoursCount { get; set; }

        public int ResubmittedCount { get; set; }

        public List<DashboardVacancySummaryViewModel> Vacancies { get; set; }

        public List<RegionalTeamMetrics> RegionalTeamsMetrics { get; set; }
    }
}