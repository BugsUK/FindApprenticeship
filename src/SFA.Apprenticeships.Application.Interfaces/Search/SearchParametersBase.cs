namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;

    public abstract class SearchParametersBase
    {
        public IEnumerable<int> ExcludeVacancyIds { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
