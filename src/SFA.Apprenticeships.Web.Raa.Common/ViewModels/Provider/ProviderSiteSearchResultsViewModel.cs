namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.Collections.Generic;

    public class ProviderSiteSearchResultsViewModel
    {
        public ProviderSiteSearchViewModel SearchViewModel { get; set; }

        public IList<ProviderSiteViewModel> ProviderSites { get; set; }
    }
}