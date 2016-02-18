using System.Collections.Generic;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderViewModel SaveProviderViewModel(string ukprn, ProviderViewModel providerViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(string ukprn, string ern);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
        ProviderSiteEmployerLinkViewModel GetProviderSiteEmployerLinkViewModel(int providerSiteId, int employerId);
        ProviderSiteEmployerLinkViewModel ConfirmProviderSiteEmployerLink(ProviderSiteEmployerLinkViewModel viewModel);
        EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(string providerSiteErn);
        EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(EmployerSearchViewModel viewModel);
    }
}