namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    public class DashboardVacancySummariesSearchViewModel
    {
        public DashboardVacancySummaryFilterTypes FilterType { get; set; }

        public DashboardVacancySummariesSearchViewModel()
        {
            
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewMode) : this()
        {
            
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel, DashboardVacancySummaryFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
        }
    }
}