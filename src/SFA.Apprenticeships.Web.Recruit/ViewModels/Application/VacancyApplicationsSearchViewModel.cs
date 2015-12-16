namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class VacancyApplicationsSearchViewModel
    {
        public VacancyApplicationsSearchViewModel()
        {
            PageSize = 25;
            CurrentPage = 1;
        }

        private VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel) : this()
        {
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = viewModel.FilterType;
            PageSize = viewModel.PageSize;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, VacancyApplicationsFilterTypes filterType) : this(viewModel)
        {
            FilterType = filterType;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, int currentPage) : this(viewModel)
        {
            CurrentPage = currentPage;
        }

        public long VacancyReferenceNumber { get; set; }
        public VacancyApplicationsFilterTypes FilterType { get; set; }
        public int PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; set; }
        public int CurrentPage { get; set; }
    }
}