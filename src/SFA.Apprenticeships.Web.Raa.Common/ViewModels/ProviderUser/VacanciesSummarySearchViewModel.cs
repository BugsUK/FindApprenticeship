namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class VacanciesSummarySearchViewModel : OrderedPageableSearchViewModel
    {
        public static readonly string OrderByFieldTitle = VacancySummaryOrderByColumn.Title.ToString();
        public static readonly string OrderByEmployer = VacancySummaryOrderByColumn.Employer.ToString();
        public static readonly string OrderByLocation = VacancySummaryOrderByColumn.Location.ToString();
        public static readonly string OrderByApplications = VacancySummaryOrderByColumn.Applications.ToString();

        public VacanciesSummarySearchViewModel()
        {
            VacancyType = VacancyType.Apprenticeship;
        }

        internal VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel) : base(viewModel)
        {
            VacancyType = viewModel.VacancyType;
            FilterType = viewModel.FilterType;
            SearchString = viewModel.SearchString;
            PageSize = viewModel.PageSize;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacanciesSummaryFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
            CurrentPage = 1;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacanciesSummaryFilterTypes filterType, string searchString) : this(viewModel, filterType)
        {
            SearchString = searchString;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacancyType vacancyType) : this(viewModel)
        {
            VacancyType = vacancyType;
            SearchString = null;
            CurrentPage = 1;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, string orderByField, Order order) : this(viewModel)
        {
            OrderByField = orderByField;
            Order = order;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, int currentPage) : this(viewModel)
        {
            CurrentPage = currentPage;
        }

        public VacancyType VacancyType { get; set; }
        public VacanciesSummaryFilterTypes FilterType { get; set; }
        public VacancySearchMode SearchMode { get; set; }
        public string SearchString { get; set; }

        public override object RouteValues => new
        {
            VacancyType,
            FilterType,
            SearchMode,
            SearchString,
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };
    }
}