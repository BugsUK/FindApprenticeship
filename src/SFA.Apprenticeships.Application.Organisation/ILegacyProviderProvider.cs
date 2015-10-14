using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Providers;

    public interface ILegacyProviderProvider
    {
        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string ern);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern);
        
        IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(EmployerSearchRequest searchRequest);
    }
}