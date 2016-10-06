namespace SFA.Apprenticeships.Web.Raa.Common.Mediators.Admin
{
    using ViewModels.Admin;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using Web.Common.Mediators;

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
        MediatorResponse<TransferVacanciesResultsViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel);
        MediatorResponse<ManageVacancyTransferResultsViewModel> ManageVacanciesTransfers(ManageVacancyTransferViewModel vacancyTransferViewModel);
        MediatorResponse<ProviderUserSearchResultsViewModel> SearchProviderUsers(ProviderUserSearchViewModel searchViewModel, string ukprn);
    }
}