using System.Collections.Generic;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.Employer;

    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn, bool errorIfNotFound = true);
        ProviderViewModel GetProviderViewModel(int providerId);
        ProviderSearchResultsViewModel SearchProviders(ProviderSearchViewModel searchViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(int providerSiteId);
        ProviderSiteViewModel GetProviderSiteViewModel(string edsUrn);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
        ProviderSiteSearchResultsViewModel SearchProviderSites(ProviderSiteSearchViewModel searchViewModel);
        VacancyOwnerRelationshipViewModel GetVacancyOwnerRelationshipViewModel(int vacancyOwnerRelationshipId);
        VacancyOwnerRelationshipViewModel GetVacancyOwnerRelationshipViewModel(int providerSiteId, string edsUrn);
        VacancyOwnerRelationshipViewModel ConfirmVacancyOwnerRelationship(VacancyOwnerRelationshipViewModel viewModel);
        EmployerSearchViewModel GetVacancyOwnerRelationshipViewModels(int providerSiteId);
        EmployerSearchViewModel GetVacancyOwnerRelationshipViewModels(EmployerSearchViewModel viewModel);
        ProviderViewModel CreateProvider(ProviderViewModel viewModel);
        ProviderViewModel SaveProvider(ProviderViewModel viewModel);
        ProviderSiteViewModel CreateProviderSite(ProviderSiteViewModel viewModel);
        ProviderSiteViewModel SaveProviderSite(ProviderSiteViewModel viewModel);
        ProviderSiteViewModel CreateProviderSiteRelationship(ProviderSiteViewModel viewModel, int providerId);
        ProviderSiteRelationshipViewModel GetProviderSiteRelationshipViewModel(int providerSiteRelationshipId);
        void DeleteProviderSiteRelationship(int providerSiteRelationshipId);
    }
}