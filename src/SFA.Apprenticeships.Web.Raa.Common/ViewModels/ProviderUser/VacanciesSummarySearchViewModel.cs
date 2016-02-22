namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Entities.Vacancies;

    public class VacanciesSummarySearchViewModel
    {
        public VacanciesSummarySearchViewModel()
        {
            VacancyType = VacancyType.Apprenticeship;
            ShowAllLotteryNumbers = true;
            PageSize = 25;
            CurrentPage = 1;
        }

        private VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel) : this()
        {
            VacancyType = viewModel.VacancyType;
            FilterType = viewModel.FilterType;
            ShowAllLotteryNumbers = viewModel.ShowAllLotteryNumbers;
            SearchString = viewModel.SearchString;
            PageSize = viewModel.PageSize;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacanciesSummaryFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
            SearchString = null;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, bool showAllLotteryNumbers) : this(viewModel)
        {
            ShowAllLotteryNumbers = showAllLotteryNumbers;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, VacancyType vacancyType) : this(viewModel)
        {
            VacancyType = vacancyType;
            SearchString = null;
        }

        public VacanciesSummarySearchViewModel(VacanciesSummarySearchViewModel viewModel, int currentPage) : this(viewModel)
        {
            CurrentPage = currentPage;
        }

        public VacancyType VacancyType { get; set; }
        public VacanciesSummaryFilterTypes FilterType { get; set; }
        public bool ShowAllLotteryNumbers { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; set; }
        public int CurrentPage { get; set; }
    }
}