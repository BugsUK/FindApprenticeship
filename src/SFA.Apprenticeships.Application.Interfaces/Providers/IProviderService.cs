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
        Provider GetProvider(int providerId);

        Provider GetProvider(string ukprn);

        IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds);

        ProviderSite GetProviderSite(int providerSiteId);

        ProviderSite GetProviderSite(string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(int providerId);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IReadOnlyDictionary<int, ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds);

        IEnumerable<ProviderSite> GetOwnedProviderSites(int providerId);

        VacancyParty GetVacancyParty(int vacancyPartyId, bool currentOnly);

        IReadOnlyDictionary<int, VacancyParty> GetVacancyParties(IEnumerable<int> vacancyPartyIds, bool currentOnly);

        VacancyParty GetVacancyParty(int providerSiteId, string edsUrn);

        bool IsADeletedVacancyParty(int providerSiteId, string edsUrn);

        void ResurrectVacancyParty(int providerSiteId, string edsUrn);

        VacancyParty SaveVacancyParty(VacancyParty vacancyParty);
        
        IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId);

        Pageable<VacancyParty> GetVacancyParties(EmployerSearchRequest request, int currentPage, int pageSize);        
    }
}
