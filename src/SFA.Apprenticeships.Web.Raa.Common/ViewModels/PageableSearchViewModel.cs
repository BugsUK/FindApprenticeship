namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Factories;

    public class PageableSearchViewModel
    {
        private int _pageSize;

        public PageableSearchViewModel() : this(25, 1)
        {

        }

        public PageableSearchViewModel(int pageSize, int currentPage)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
        }

        public PageableSearchViewModel(PageableSearchViewModel viewModel) : this(viewModel.PageSize, viewModel.CurrentPage)
        {
            
        }

        public PageableSearchViewModel(PageableSearchViewModel viewModel, int currentPage) : this(viewModel.PageSize, currentPage)
        {
            
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                PageSizes = SelectListItemsFactory.GetPageSizes(_pageSize);
            }
        }

        public List<SelectListItem> PageSizes { get; set; }
        public int CurrentPage { get; set; }

        public virtual object RouteValues => new
        {
            PageSize,
            CurrentPage
        };
    }
}