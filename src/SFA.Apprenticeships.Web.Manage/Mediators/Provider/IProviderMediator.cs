using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Manage.Mediators.Provider
{
    using ViewModels.Provider;

    public interface IProviderMediator
    {
        MediatorResponse<ProviderSiteSearchViewModel> AddSite();

        MediatorResponse<ProviderSiteSearchViewModel> AddSite(ProviderSiteSearchViewModel viewModel);

        MediatorResponse<ProviderViewModel> Sites(string ukprn);

        MediatorResponse<ProviderViewModel> UpdateSites(string ukprn, string username, ProviderViewModel providerViewModel);

        MediatorResponse<ProviderSiteViewModel> GetSite(string ern);

        MediatorResponse<ProviderSiteViewModel> UpdateSite(ProviderSiteViewModel providerSiteViewModel);
    }
}