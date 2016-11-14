namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.Collections.Generic;

    public class ProviderSearchResultsViewModel
    {
        public ProviderSearchViewModel SearchViewModel { get; set; }

        public IList<ProviderViewModel> Providers { get; set; }

        public IList<int> VacancyReferenceNumbers { get; set; }
        
    }
}