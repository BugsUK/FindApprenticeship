using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    using SFA.Apprenticeships.Domain.Entities.Communication;

    /// <summary>
    /// For maintaining provider profiles, sites, etc.
    /// </summary>
    public interface IProviderService
    {
        Provider GetProviderViaOwnerParty(int vacancyPartyId);

        Provider GetProvider(int providerId);

        Provider GetProvider(string ukprn);

        IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds);

        ProviderSite GetProviderSite(int providerSiteId);

        ProviderSite GetProviderSite(string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds);

        VacancyParty GetVacancyParty(int vacancyPartyId);

        VacancyParty GetVacancyParty(int providerSiteId, string edsUrn);

        VacancyParty SaveVacancyParty(VacancyParty vacancyParty);

        IEnumerable<VacancyParty> GetVacancyParties(IEnumerable<int> vacancyPartyIds);

        IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId);

        Pageable<VacancyParty> GetVacancyParties(EmployerSearchRequest request, int currentPage, int pageSize);

        void SubmitContactMessage(ContactMessage contactMessage);
    }
}
