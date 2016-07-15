using System.Collections.Generic;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public interface IProviderProvider
    {
        ProviderViewModel GetProviderViewModel(string ukprn);
        ProviderViewModel GetProviderViewModel(int providerId);
        ProviderSiteViewModel GetProviderSiteViewModel(string edsUrn);
        IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn);
        VacancyPartyViewModel GetVacancyPartyViewModel(int vacancyPartyId);
        VacancyPartyViewModel GetVacancyPartyViewModel(int providerSiteId, string edsUrn);
        VacancyPartyViewModel ConfirmVacancyParty(VacancyPartyViewModel viewModel);
        EmployerSearchViewModel GetVacancyPartyViewModels(int providerSiteId);
        EmployerSearchViewModel GetVacancyPartyViewModels(EmployerSearchViewModel viewModel);
    }
}