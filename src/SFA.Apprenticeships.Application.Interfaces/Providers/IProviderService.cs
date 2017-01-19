using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;

    /// <summary>
    /// For maintaining provider profiles, sites, etc.
    /// </summary>
    public interface IProviderService
    {
        Provider GetProvider(int providerId);

        Provider GetProvider(string ukprn, bool errorIfNotFound = true);

        IEnumerable<Provider> GetProviders(IEnumerable<int> providerIds);

        IEnumerable<Provider> SearchProviders(ProviderSearchParameters searchParameters);

        ProviderSite GetProviderSite(int providerSiteId);

        ProviderSite GetProviderSite(string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(int providerId);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IReadOnlyDictionary<int, ProviderSite> GetProviderSites(IEnumerable<int> providerSiteIds);

        IEnumerable<ProviderSite> GetOwnedProviderSites(int providerId);

        IEnumerable<ProviderSite> SearchProviderSites(ProviderSiteSearchParameters searchParameters);

        VacancyOwnerRelationship GetVacancyOwnerRelationship(int vacancyOwnerRelationshipId, bool currentOnly);

        VacancyOwnerRelationship GetVacancyOwnerRelationship(int employerId, int providerSiteId, bool liveOnly);

        IReadOnlyDictionary<int, VacancyOwnerRelationship> GetVacancyOwnerRelationships(IEnumerable<int> vacancyOwnerRelationshipIds, bool currentOnly);

        VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, string edsUrn, bool liveOnly);

        VacancyOwnerRelationship SaveVacancyOwnerRelationship(VacancyOwnerRelationship vacancyOwnerRelationship);

        IEnumerable<VacancyOwnerRelationship> GetVacancyOwnerRelationships(int providerSiteId);

        Pageable<VacancyOwnerRelationship> GetVacancyOwnerRelationships(EmployerSearchRequest request, int currentPage, int pageSize);

        Provider CreateProvider(Provider provider);

        Provider SaveProvider(Provider provider);

        ProviderSite CreateProviderSite(ProviderSite providerSite);

        ProviderSite SaveProviderSite(ProviderSite providerSite);

        ProviderSiteRelationship GetProviderSiteRelationship(int providerSiteRelationshipId);

        ProviderSiteRelationship CreateProviderSiteRelationship(ProviderSiteRelationship providerSiteRelationship);

        void DeleteProviderSiteRelationship(int providerSiteRelationshipId);
    }
}
