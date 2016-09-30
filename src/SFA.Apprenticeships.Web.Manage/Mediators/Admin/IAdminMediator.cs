namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using System;
    using Common.Mediators;
    using Raa.Common.ViewModels.Api;
    using Raa.Common.ViewModels.Provider;

    public interface IAdminMediator
    {
        MediatorResponse<ProviderSearchResultsViewModel> SearchProviders(ProviderSearchViewModel searchViewModel);
        MediatorResponse<ProviderViewModel> GetProvider(int providerId);
        MediatorResponse<ProviderViewModel> CreateProvider(ProviderViewModel viewModel);
        MediatorResponse<ProviderSiteSearchResultsViewModel> SearchProviderSites(ProviderSiteSearchViewModel searchViewModel);
        MediatorResponse<ProviderSiteViewModel> GetProviderSite(int providerSiteId);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSite(ProviderSiteViewModel viewModel);
        MediatorResponse<ProviderSiteViewModel> SaveProviderSite(ProviderSiteViewModel viewModel);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSiteRelationship(ProviderSiteViewModel viewModel);
        MediatorResponse<ApiUserSearchResultsViewModel> SearchApiUsers(ApiUserSearchViewModel searchViewModel);
        MediatorResponse<ApiUserViewModel> GetApiUser(Guid externalSystemId);
    }
}