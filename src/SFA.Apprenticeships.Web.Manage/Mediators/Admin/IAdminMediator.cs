namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Provider;

    public interface IAdminMediator
    {
        MediatorResponse<ProviderSearchResultsViewModel> SearchProviders(ProviderSearchViewModel searchViewModel);
        MediatorResponse<ProviderViewModel> GetProvider(int providerId);
        MediatorResponse<ProviderViewModel> CreateProvider(ProviderViewModel viewModel);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSite(ProviderSiteViewModel viewModel);
    }
}