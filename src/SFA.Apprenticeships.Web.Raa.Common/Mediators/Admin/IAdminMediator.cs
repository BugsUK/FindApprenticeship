namespace SFA.Apprenticeships.Web.Raa.Common.Mediators.Admin
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Admin;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using ViewModels.Api;
    using ViewModels.Employer;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using Web.Common.Mediators;

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
        MediatorResponse<ProviderSiteRelationshipViewModel> GetProviderSiteRelationship(int providerSiteRelationshipId);
        MediatorResponse<ProviderSiteViewModel> CreateProviderSiteRelationship(ProviderSiteViewModel viewModel);
        MediatorResponse<ProviderSiteRelationshipViewModel> DeleteProviderSiteRelationship(int providerSiteRelationshipId);
        MediatorResponse<ApiUserSearchResultsViewModel> SearchApiUsers(ApiUserSearchViewModel searchViewModel);
        MediatorResponse<ApiUserViewModel> GetApiUser(Guid externalSystemId);
        MediatorResponse<ApiUserViewModel> CreateApiUser(ApiUserViewModel viewModel);
        MediatorResponse<ApiUserViewModel> SaveApiUser(ApiUserViewModel viewModel);
        MediatorResponse<ApiUserViewModel> ResetApiUserPassword(ApiUserViewModel viewModel);
        MediatorResponse<byte[]> GetApiUsersBytes();
        MediatorResponse<TransferVacanciesResultsViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel);
        MediatorResponse<ManageVacancyTransferResultsViewModel> ManageVacanciesTransfers(ManageVacancyTransferViewModel vacancyTransferViewModel);
        MediatorResponse<ProviderUserSearchResultsViewModel> SearchProviderUsers(ProviderUserSearchViewModel searchViewModel, string ukprn);
        MediatorResponse<ProviderUserViewModel> GetProviderUser(int providerUserId);
        MediatorResponse<ProviderUserViewModel> SaveProviderUser(ProviderUserViewModel viewModel);
        MediatorResponse<ProviderUserViewModel> VerifyProviderUserEmail(ProviderUserViewModel viewModel);
        MediatorResponse<EmployerSearchViewModel> SearchEmployers(EmployerSearchViewModel searchViewModel);
        MediatorResponse<EmployerViewModel> GetEmployer(int employerId);
        MediatorResponse<EmployerViewModel> SaveEmployer(EmployerViewModel viewModel);
        MediatorResponse<List<StandardSubjectAreaTierOne>> GetStandard();
        MediatorResponse<Standard> UpdateStandard(Standard standard);
        MediatorResponse<List<Category>> GetFrameworks();
        MediatorResponse<byte[]> GetFrameworksBytes();
        MediatorResponse<byte[]> GetStandardsBytes();
    }
}