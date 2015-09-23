using SFA.Apprenticeships.Web.Manage.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Collections.Generic;

    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderViewModel SaveProviderViewModel(string ukprn, ProviderViewModel providerViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(string ern);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
    }
}