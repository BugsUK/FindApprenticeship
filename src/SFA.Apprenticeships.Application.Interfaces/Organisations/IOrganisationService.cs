using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;

    public interface IOrganisationService
    {
        VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber);

        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string ern);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        ProviderSiteEmployerLink GetProviderSiteEmployerLink(string providerSiteErn, string ern);

        IEnumerable<ProviderSiteEmployerLink> GetProviderSiteEmployerLinks(EmployerSearchRequest request);

        Employer GetEmployer(string ern);
    }
}
