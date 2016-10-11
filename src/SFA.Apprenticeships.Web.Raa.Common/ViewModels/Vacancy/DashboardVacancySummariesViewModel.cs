namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class DashboardVacancySummariesViewModel
    {
        public DashboardVacancySummariesSearchViewModel SearchViewModel { get; set; }

        public int SubmittedTodayCount { get; set; }

        public int SubmittedYesterdayCount { get; set; }

        public int SubmittedMoreThan48HoursCount { get; set; }

        public int ResubmittedCount { get; set; }

        public List<DashboardVacancySummaryViewModel> Vacancies { get; set; }

        public IList<RegionalTeamMetrics> RegionalTeamsMetrics { get; set; }
    }
}