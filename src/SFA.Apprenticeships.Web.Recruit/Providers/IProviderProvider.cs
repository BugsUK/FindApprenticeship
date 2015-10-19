using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Collections.Generic;

    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderViewModel SaveProviderViewModel(string ukprn, ProviderViewModel providerViewModel);
        ProviderSiteViewModel GetProviderSiteViewModel(string ukprn, string ern);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
        ProviderSiteEmployerLinkViewModel GetProviderSiteEmployerLinkViewModel(string providerSiteErn, string ern);
        ProviderSiteEmployerLinkViewModel ConfirmProviderSiteEmployerLink(ProviderSiteEmployerLinkViewModel viewModel);
        EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(string providerSiteErn);
        EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(EmployerSearchViewModel viewModel);
    }
}