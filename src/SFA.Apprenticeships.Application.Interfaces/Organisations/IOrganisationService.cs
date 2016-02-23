using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;
    using Generic;

    public interface IOrganisationService
    {
        VerifiedOrganisationSummary GetVerifiedOrganisationSummary(string referenceNumber);

        IEnumerable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location);

        Pageable<VerifiedOrganisationSummary> GetVerifiedOrganisationSummaries(string edsUrn, string name, string location, int currentPage, int pageSize);

        //TODO: Remove all these methods once we have migrated the data layer
        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        VacancyParty GetVacancyParty(int providerSiteId, int employerId);

        VacancyParty GetVacancyParty(int providerSiteId, string edsUrn);

        IEnumerable<VacancyParty> GetProviderSiteEmployerLinks(EmployerSearchRequest request);

        Employer GetEmployer(int employerId);

        Employer GetEmployer(string edsUrn);

        IEnumerable<Employer> GetByIds(IEnumerable<int> employerIds);

        IEnumerable<Employer> GetEmployers(string edsUrn, string name, string location);

        Pageable<Employer> GetEmployers(string edsUrn, string name, string location, int currentPage, int pageSize);
    }
}
