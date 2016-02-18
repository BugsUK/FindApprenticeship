namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Providers;

    public interface IProviderSiteReadRepository { 
        IEnumerable<ProviderSite> GetForProvider(string ukprn);
        ProviderSite Get(string ern);
    }

    public interface IProviderSiteWriteRepository
    {
        ProviderSite Save(ProviderSite entity);
    }
}