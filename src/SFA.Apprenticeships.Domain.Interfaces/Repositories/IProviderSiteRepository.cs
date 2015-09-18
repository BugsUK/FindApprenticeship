namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Providers;

    public interface IProviderSiteReadRepository : IReadRepository<ProviderSite>
    {
        IEnumerable<ProviderSite> GetForProvider(string ukprn);
    }

    public interface IProviderSiteWriteRepository : IWriteRepository<ProviderSite> { }
}