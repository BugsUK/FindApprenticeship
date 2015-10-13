namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;

    public interface IOrganisationService
    {
        Organisation GetByReferenceNumber(string referenceNumber);

        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string ern);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern);

        IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(string providerSiteErn);

        Employer GetEmployer(string ern);
    }
}
