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
        Provider GetProviderViaCurrentOwnerParty(int vacancyPartyId);

        Provider GetProvider(int providerId);

        Provider GetProvider(string ukprn);

        IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds);

        ProviderSite GetProviderSite(int providerSiteId);

        ProviderSite GetProviderSite(string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyPartyId"></param>
        /// <param name="currentOnly">Set to "true" when creating / editing records. Set to "false" when displaying records which may be historic.
        /// Also set to false when displaying current vacancies migrated from AVMS as it allows vacancies party entities to be removed even when in use for current vacancies.</param>
        /// <returns></returns>
        VacancyParty GetVacancyParty(int vacancyPartyId, bool currentOnly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyPartyIds"></param>
        /// <param name="currentOnly">Set to "true" when creating / editing records. Set to "false" when displaying records which may be historic.
        /// Also set to false when displaying current vacancies migrated from AVMS as it allows vacancies party entities to be removed even when in use for current vacancies.</param>
        /// <returns></returns>
        IReadOnlyDictionary<int, VacancyParty> GetVacancyParties(IEnumerable<int> vacancyPartyIds, bool currentOnly);

        VacancyParty GetVacancyParty(int providerSiteId, string edsUrn);

        VacancyParty SaveVacancyParty(VacancyParty vacancyParty);


        IEnumerable<VacancyParty> GetAllVacancyPartiesByProviderSite(int providerSiteId);

        Pageable<VacancyParty> GetVacancyParties(EmployerSearchRequest request, int currentPage, int pageSize);        
    }
}
