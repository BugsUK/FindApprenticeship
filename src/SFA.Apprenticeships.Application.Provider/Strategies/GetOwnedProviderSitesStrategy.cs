namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    public class GetOwnedProviderSitesStrategy : IGetOwnedProviderSitesStrategy
    {
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;

        public GetOwnedProviderSitesStrategy(IProviderSiteReadRepository providerSiteReadRepository)
        {
            _providerSiteReadRepository = providerSiteReadRepository;
        }

        public IEnumerable<ProviderSite> GetOwnedProviderSites(int providerId)
        {
            var providerSites = _providerSiteReadRepository.GetByProviderId(providerId);
            return providerSites.Where(ps => ps.ProviderSiteRelationships.Any(psr => psr.ProviderId == providerId && psr.ProviderSiteRelationShipTypeId == ProviderSiteRelationshipTypes.Owner));
        }
    }
}