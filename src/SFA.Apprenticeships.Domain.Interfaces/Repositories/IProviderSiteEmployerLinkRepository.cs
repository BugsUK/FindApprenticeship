namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Providers;

    public interface IProviderSiteEmployerLinkReadRepository
    {
        ProviderSiteEmployerLink Get(string providerSiteErn, string ern);
        IEnumerable<ProviderSiteEmployerLink> GetForProviderSite(string providerSiteErn);
    }

    public interface IProviderSiteEmployerLinkWriteRepository
    {
        ProviderSiteEmployerLink Save(ProviderSiteEmployerLink providerSiteEmployerLink);
    }
}