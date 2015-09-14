using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using ViewModels.Provider;

    public interface IProviderSiteMediator
    {
        MediatorResponse<ProviderSiteSearchResponseViewModel> FindSite(ProviderSiteSearchViewModel searchViewModel);
    }
}
