namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using System.Collections.Generic;

    public class PageableViewModel<T>
    {
        public PageableViewModel()
        {
            CurrentPage = 1;
            TotalNumberOfPages = 1;
        }

        public IEnumerable<T> Page { get; set; }

        public int ResultsCount { get; set; }

        public int CurrentPage { get; set; }

        public int TotalNumberOfPages { get; set; }
    }
}