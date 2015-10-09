using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;

    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderViewModel SaveProviderViewModel(string ukprn, ProviderViewModel providerViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(string ukprn, string ern);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
    }
}