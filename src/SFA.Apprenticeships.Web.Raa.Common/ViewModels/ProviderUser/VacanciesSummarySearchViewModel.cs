namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class VacanciesSummarySearchViewModel
    {
        public VacanciesSummarySearchViewModel()
        {
            PageSize = 10;
            CurrentPage = 1;
        }

        private VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel) : this()
        {
            FilterType = viewModel.FilterType;
            SearchString = viewModel.SearchString;
            PageSize = viewModel.PageSize;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacanciesSummaryFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, int currentPage) : this(viewModel)
        {
            CurrentPage = currentPage;
        }

        public VacanciesSummaryFilterTypes FilterType { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; set; }
        public int CurrentPage { get; set; }
    }
}