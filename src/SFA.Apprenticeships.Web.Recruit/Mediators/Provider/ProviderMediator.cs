using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Provider
{
    using ViewModels.Provider;

    public class ProviderMediator : IProviderMediator
    {
        public MediatorResponse<ProviderSiteSearchViewModel> AddSite()
        {
            return null;
        }

        public MediatorResponse<ProviderSiteSearchResponseViewModel> FindSite(ProviderSiteSearchViewModel searchViewModel)
        {
            return null;
        }
    }
}
