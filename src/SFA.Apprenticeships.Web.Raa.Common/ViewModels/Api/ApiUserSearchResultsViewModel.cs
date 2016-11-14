namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Api
{
    using System.Collections.Generic;

    public class ApiUserSearchResultsViewModel
    {
        public ApiUserSearchViewModel SearchViewModel { get; set; }

        public IList<ApiUserViewModel> ApiUsers { get; set; }
    }
}