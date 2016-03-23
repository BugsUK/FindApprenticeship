namespace SFA.Apprenticeships.Web.Common.Converters
{
    using System.Collections.Generic;
    using Application.Interfaces.Generic;
    using ViewModels;

    public static class PageableConverters
    {
        public static PageableViewModel<T> ToViewModel<T>(this Pageable<T> pageable)
        {
            var viewModel = new PageableViewModel<T>
            {
                Page = pageable.Page,
                ResultsCount = pageable.ResultsCount,
                CurrentPage = pageable.CurrentPage,
                TotalNumberOfPages = pageable.TotalNumberOfPages
            };
            return viewModel;
        }

        public static PageableViewModel<TViewModel> ToViewModel<T, TViewModel>(this Pageable<T> pageable, IEnumerable<TViewModel> page)
        {
            var viewModel = new PageableViewModel<TViewModel>
            {
                Page = page,
                ResultsCount = pageable.ResultsCount,
                CurrentPage = pageable.CurrentPage,
                TotalNumberOfPages = pageable.TotalNumberOfPages
            };
            return viewModel;
        }
    }
}