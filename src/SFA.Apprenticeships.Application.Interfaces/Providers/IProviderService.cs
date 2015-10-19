using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Generic;

namespace SFA.Apprenticeships.Application.Interfaces.Providers
{
    using System.Collections.Generic;
    using Domain.Entities.Providers;

    /// <summary>
    /// For maintaining provider profiles, sites, etc.
    /// </summary>
    public interface IProviderService
    {
        Provider GetProvider(string ukprn);

        void SaveProvider(Provider provider);

        ProviderSite GetProviderSite(string ukprn, string ern);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        void SaveProviderSites(IEnumerable<ProviderSite> providerSites);

        ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern);

        ProviderSiteEmployerLink SaveProviderSiteEmployerLink(ProviderSiteEmployerLink providerSiteEmployerLink);
        
        Pageable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(EmployerSearchRequest request, int currentPage, int pageSize);
    }
}
