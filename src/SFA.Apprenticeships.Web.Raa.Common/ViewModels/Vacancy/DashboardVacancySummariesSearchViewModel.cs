namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    public class DashboardVacancySummariesSearchViewModel : OrderedSearchViewModel
    {
        public const string OrderByFieldTitle = "Title";
        public const string OrderByFieldProvider = "Provider";
        public const string OrderByFieldDateSubmitted = "DateSubmitted";
        public const string OrderByFieldClosingDate = "ClosingDate";
        public const string OrderByFieldSubmissionCount = "SubmissionCount";

        public DashboardVacancySummaryFilterTypes FilterType { get; set; }
        public DashboardVacancySummariesMode Mode { get; set; }

        public DashboardVacancySummariesSearchViewModel()
        {
            
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel) : base(viewModel)
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

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel, string orderByField, Order order) : base(orderByField, order)
        {
            FilterType = viewModel.FilterType;
            Mode = viewModel.Mode;
        }
    }
}