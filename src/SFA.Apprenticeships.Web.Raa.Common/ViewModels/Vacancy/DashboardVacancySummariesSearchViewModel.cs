namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Domain.Raa.Interfaces.Repositories.Models;
    using Factories;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class DashboardVacancySummariesSearchViewModel : OrderedSearchViewModel
    {
        public const string OrderByFieldTitle = "Title";
        public const string OrderByFieldProvider = "Provider";
        public const string OrderByFieldDateSubmitted = "DateSubmitted";
        public const string OrderByFieldClosingDate = "ClosingDate";
        public const string OrderByFieldSubmissionCount = "SubmissionCount";
        public const string OrderByFieldVacancyLocation = "VacancyLocation";

        public VacanciesSummaryFilterTypes FilterType { get; set; }
        public DashboardVacancySummariesMode Mode { get; set; }
        public string SearchString { get; set; }
        public List<SelectListItem> SearchModes => SelectListItemsFactory.GetManageSearchModes(SearchMode);
        public ManageVacancySearchMode SearchMode { get; set; }

        public DashboardVacancySummariesSearchViewModel()
        {

        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel) : base(viewModel)
        {
            SetValues(viewModel);
        }

        public DashboardVacancySummariesSearchViewModel(DashboardVacancySummariesSearchViewModel viewModel, VacanciesSummaryFilterTypes filterType) : this(viewModel)
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
            SetValues(viewModel);
        }

        private void SetValues(DashboardVacancySummariesSearchViewModel viewModel)
        {
            FilterType = viewModel.FilterType;
            Mode = viewModel.Mode;
            SearchString = viewModel.SearchString;
            SearchMode = viewModel.SearchMode;
        }
    }
}