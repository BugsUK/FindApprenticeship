namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;
    using Models;

    public interface IProviderSiteReadRepository
    {
        ProviderSite GetById(int providerSiteId);

        ProviderSite GetByEdsUrn(string edsUrn);

        IReadOnlyDictionary<int, ProviderSite> GetByIds(IEnumerable<int> providerSiteIds);

        IEnumerable<ProviderSite> GetByProviderId(int providerId);

        IEnumerable<ProviderSite> Search(ProviderSiteSearchParameters searchParameters);
    }

    public interface IProviderSiteWriteRepository
    {
        ProviderSite Create(ProviderSite providerSite);
        ProviderSite Update(ProviderSite providerSite);
    }
}
