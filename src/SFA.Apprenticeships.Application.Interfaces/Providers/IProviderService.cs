using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    /// <summary>
    /// For maintaining provider profiles, sites, etc.
    /// </summary>
    public interface IProviderService
    {
        Provider GetProviderViaOwnerParty(int vacancyPartyId);

        Provider GetProvider(string ukprn);

        IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds);

        void SaveProvider(Provider provider);

        ProviderSite GetProviderSite(int providerSiteId);

        ProviderSite GetProviderSite(string ukprn, string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds);

        void SaveProviderSites(IEnumerable<ProviderSite> providerSites);

        VacancyParty GetVacancyParty(int vacancyPartyId);

        VacancyParty GetVacancyParty(int providerSiteId, string edsUrn);

        VacancyParty SaveVacancyParty(VacancyParty vacancyParty);

        IEnumerable<VacancyParty> GetVacancyParties(IEnumerable<int> vacancyPartyIds);

        IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId);

        Pageable<VacancyParty> GetVacancyParties(EmployerSearchRequest request, int currentPage, int pageSize);
    }
}
