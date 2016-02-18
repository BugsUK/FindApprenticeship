namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Providers;
    using Domain.Interfaces.Repositories;

    public class ProviderSiteRepository : IProviderSiteReadRepository, IProviderSiteWriteRepository
    {
        public IEnumerable<ProviderSite> GetForProvider(string ukprn)
        {
            throw new NotImplementedException();
        }

        public ProviderSite Get(string ern)
        {
            throw new NotImplementedException();
        }

        public ProviderSite Save(ProviderSite entity)
        {
            throw new NotImplementedException();
        }
    }
}
