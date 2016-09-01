namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderSiteReadRepository
    {
        ProviderSite GetById(int providerSiteId);

        ProviderSite GetByEdsUrn(string edsUrn);

        IReadOnlyDictionary<int, ProviderSite> GetByIds(IEnumerable<int> providerSiteIds);

        IEnumerable<ProviderSite> GetByProviderId(int providerId, bool returnAll = false);
    }

    public interface IProviderSiteWriteRepository
    {
        ProviderSite Update(ProviderSite providerSite);
    }
}
