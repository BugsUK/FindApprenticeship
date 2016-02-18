using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;
    using Generic;

    public interface IOrganisationService
    {
        VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber);

        IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string ern, string name, string location);

        Pageable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string ern, string name, string location, int currentPage, int pageSize);

        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string ern);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        VacancyParty GetVacancyParty(int providerSiteId, int employerId);

        VacancyParty GetVacancyParty(string providerSiteErn, string ern);

        IEnumerable<VacancyParty> GetProviderSiteEmployerLinks(EmployerSearchRequest request);

        Employer GetEmployer(int employerId);

        Employer GetEmployer(string ern);

        IEnumerable<Employer> GetEmployers(string ern, string name, string location);

        Pageable<Employer> GetEmployers(string ern, string name, string location, int currentPage, int pageSize);
    }
}
