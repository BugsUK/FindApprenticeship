namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;
    using Raa.Common.ViewModels.Vacancy;

    public class DashboardVacancySummariesViewModel
    {
        public DashboardVacancySummariesSearchViewModel SearchViewModel { get; set; }

        public int SubmittedTodayCount { get; set; }

        public int SubmittedYesterdayCount { get; set; }

        public int SubmittedMoreThan48HoursCount { get; set; }

        public int ResubmittedCount { get; set; }

        public List<DashboardVacancySummaryViewModel> Vacancies { get; set; }
    }
}