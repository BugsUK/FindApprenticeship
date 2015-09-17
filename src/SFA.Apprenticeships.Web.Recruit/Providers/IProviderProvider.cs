using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderSiteViewModel GetProviderSiteViewModel(string ern);
    }
}