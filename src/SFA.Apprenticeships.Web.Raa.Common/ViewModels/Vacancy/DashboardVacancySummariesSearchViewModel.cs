namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class DashboardVacancySummariesSearchViewModel
    {
        public DashboardVacancySummaryFilterTypes FilterType { get; set; }
        public DashboardVacancySummariesMode Mode { get; set; }

        public DashboardVacancySummariesSearchViewModel()
        {
            
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel) : this()
        {
            FilterType = viewModel.FilterType;
            Mode = viewModel.Mode;
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel, DashboardVacancySummaryFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
            Mode = DashboardVacancySummariesMode.Review;
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel, DashboardVacancySummariesMode mode) : this(viewModel)
        {
            Mode = mode;
        }
    }
}