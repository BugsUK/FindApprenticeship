using System.Collections.Generic;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn, bool errorIfNotFound = true);
        ProviderViewModel GetProviderViewModel(int providerId);
        ProviderSearchResultsViewModel SearchProviders(ProviderSearchViewModel searchViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(int providerSiteId);
        ProviderSiteViewModel GetProviderSiteViewModel(string edsUrn);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
        ProviderSiteSearchResultsViewModel SearchProviderSites(ProviderSiteSearchViewModel searchViewModel);
        VacancyPartyViewModel GetVacancyPartyViewModel(int vacancyPartyId);
        VacancyPartyViewModel GetVacancyPartyViewModel(int providerSiteId, string edsUrn);
        VacancyPartyViewModel ConfirmVacancyParty(VacancyPartyViewModel viewModel);
        EmployerSearchViewModel GetVacancyPartyViewModels(int providerSiteId);
        EmployerSearchViewModel GetVacancyPartyViewModels(EmployerSearchViewModel viewModel);
        ProviderViewModel CreateProvider(ProviderViewModel viewModel);
        ProviderSiteViewModel CreateProviderSite(ProviderSiteViewModel viewModel);
        ProviderSiteViewModel SaveProviderSite(ProviderSiteViewModel viewModel);
    }
}