namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    public class VacanciesSummarySearchViewModel
    {
        public VacanciesSummarySearchViewModel()
        {
            PageSize = 10;
            CurrentPage = 1;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel)
        {
            FilterType = viewModel.FilterType;
            SearchString = viewModel.SearchString;
            PageSize = viewModel.PageSize;
            CurrentPage = viewModel.CurrentPage;
        }

        public VacanciesSummaryFilterTypes FilterType { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}