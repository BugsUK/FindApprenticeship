namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
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