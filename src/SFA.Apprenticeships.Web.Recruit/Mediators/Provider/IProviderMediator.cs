namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;

    public interface IProviderMediator
    {
        MediatorResponse<ProviderSiteSearchViewModel> AddSite();

        MediatorResponse<ProviderSiteSearchViewModel> AddSite(ProviderSiteSearchViewModel viewModel);

        MediatorResponse<ProviderViewModel> Sites(string ukprn);

        MediatorResponse<ProviderViewModel> UpdateSites(string ukprn, string username, ProviderViewModel providerViewModel);

        MediatorResponse<ProviderSiteViewModel> GetSite(string ukprn, string ern);

        MediatorResponse<ProviderSiteViewModel> UpdateSite(ProviderSiteViewModel providerSiteViewModel);
    }
}