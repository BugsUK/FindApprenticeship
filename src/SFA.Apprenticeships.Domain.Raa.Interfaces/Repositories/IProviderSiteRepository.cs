namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderSiteReadRepository
    {
        IEnumerable<ProviderSite> GetForProvider(string ukprn);
        ProviderSite Get(string ern);
    }

    public interface IProviderSiteWriteRepository
    {
        void Delete(int providerSiteId);
        ProviderSite Save(ProviderSite entity);
    }
}