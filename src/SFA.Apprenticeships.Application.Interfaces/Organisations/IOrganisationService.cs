namespace SFA.Apprenticeships.Application.Interfaces.Organisations
{
    using System.Collections.Generic;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;

    public interface IOrganisationService
    {
        Organisation GetByReferenceNumber(string referenceNumber);

        Provider GetProvider(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        IEnumerable<Employer> GetEmployers(string ern);
    }
}
