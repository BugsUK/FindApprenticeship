namespace SFA.Apprenticeships.Application.Interfaces.Generic
{
    using System.Collections.Generic;

    public class Pageable<T>
    {
        public Pageable()
        {
            CurrentPage = 1;
        }

        public IEnumerable<T> Page { get; set; }

        public int ResultsCount { get; set; }

        public int CurrentPage { get; set; }

        public int TotalNumberOfPages { get; set; }
    }
}