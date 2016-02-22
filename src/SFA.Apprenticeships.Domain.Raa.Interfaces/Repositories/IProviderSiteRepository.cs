namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderSiteReadRepository
    {
        ProviderSite Get(int providerSiteId);
        ProviderSite GetByEdsErn(string edsErn);
        IEnumerable<ProviderSite> GetForProvider(string ukprn);
    }

    public interface IProviderSiteWriteRepository
    {
        void Delete(int providerSiteId);
        ProviderSite Save(ProviderSite entity);
    }
}