using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using ViewModels.Provider;

    public interface IProviderMediator
    {
        MediatorResponse<ProviderSiteSearchViewModel> AddSite();

        MediatorResponse<ProviderSiteSearchResponseViewModel> FindSite(ProviderSiteSearchViewModel searchViewModel);

        MediatorResponse<ProviderViewModel> Sites(string ukprn);

        MediatorResponse<ProviderViewModel> UpdateSites(ProviderViewModel providerViewModel);
    }
}
