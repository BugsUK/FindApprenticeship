using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using ViewModels.Provider;

    public interface IProviderMediator
    {
        MediatorResponse<ProviderSiteSearchViewModel> AddSite();

        MediatorResponse<ProviderSiteSearchViewModel> AddSite(ProviderSiteSearchViewModel viewModel);

        MediatorResponse<ProviderViewModel> Sites(string ukprn);

        MediatorResponse<ProviderViewModel> UpdateSites(ProviderViewModel providerViewModel);

        MediatorResponse<ProviderSiteViewModel> GetSite(string ern);

        MediatorResponse<ProviderSiteViewModel> UpdateSite(ProviderSiteViewModel providerSiteViewModel);
    }
}