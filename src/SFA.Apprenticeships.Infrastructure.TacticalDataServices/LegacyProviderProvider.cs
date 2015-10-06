namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using Application.Organisation;
    using Domain.Entities.Providers;

    public class LegacyProviderProvider : ILegacyProviderProvider
    {
        public Provider GetProvider(string ukprn)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            throw new System.NotImplementedException();
        }
    }
}