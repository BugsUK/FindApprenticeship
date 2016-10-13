namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices.ComTypes;
    using Common.Mediators;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Raa.Common.ViewModels.Api;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;

    public interface IAdminMediator
    {
        MediatorResponse<ProviderSearchResultsViewModel> SearchProviders(ProviderSearchViewModel searchViewModel);
        MediatorResponse<ProviderViewModel> GetProvider(int providerId);
        MediatorResponse<ProviderViewModel> CreateProvider(ProviderViewModel viewModel);
        MediatorResponse<ProviderViewModel> SaveProvider(ProviderViewModel viewModel);
        MediatorResponse<ProviderSiteSearchResultsViewModel> SearchProviderSites(ProviderSiteSearchViewModel searchViewModel);
        MediatorResponse<ProviderSiteViewModel> GetProviderSite(int providerSiteId);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSite(ProviderSiteViewModel viewModel);
        MediatorResponse<ProviderSiteViewModel> SaveProviderSite(ProviderSiteViewModel viewModel);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSiteRelationship(ProviderSiteViewModel viewModel);
        MediatorResponse<ApiUserSearchResultsViewModel> SearchApiUsers(ApiUserSearchViewModel searchViewModel);
        MediatorResponse<ApiUserViewModel> GetApiUser(Guid externalSystemId);
        MediatorResponse<ApiUserViewModel> CreateApiUser(ApiUserViewModel viewModel);
        MediatorResponse<ApiUserViewModel> SaveApiUser(ApiUserViewModel viewModel);
        MediatorResponse<List<StandardSubjectAreaTierOne>> GetStandard();
        MediatorResponse<List<Category>> GetFrameworks();
    }
}