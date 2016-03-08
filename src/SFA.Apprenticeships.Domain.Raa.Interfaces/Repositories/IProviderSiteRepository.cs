namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderSiteReadRepository
    {
        ProviderSite Get(int providerSiteId);
        ProviderSite GetByEdsUrn(string edsUrn);
        IEnumerable<ProviderSite> GetForProvider(string ukprn);
        IEnumerable<ProviderSite> GetByIds(IEnumerable<int> providerSiteIds);
    }

    public interface IProviderSiteWriteRepository
    {
        void Delete(int providerSiteId);
        ProviderSite Save(ProviderSite entity);
    }
}