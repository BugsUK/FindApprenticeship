namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using System.Collections.Generic;

    public class ProviderUserSearchResultsViewModel
    {
        public ProviderUserSearchViewModel SearchViewModel { get; set; }

        public string ProviderName { get; set; }

        public IList<ProviderUserViewModel> ProviderUsers { get; set; }
    }
}