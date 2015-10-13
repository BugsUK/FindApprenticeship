namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Providers;

    public interface IProviderSiteEmployerLinkReadRepository : IReadRepository<ProviderSiteEmployerLink>
    {
        ProviderSiteEmployerLink Get(string providerSiteErn, string ern);
        IEnumerable<ProviderSiteEmployerLink> GetForProviderSite(string providerSiteErn);
    }

    public interface IProviderSiteEmployerLinkWriteRepository : IWriteRepository<ProviderSiteEmployerLink> { }
}