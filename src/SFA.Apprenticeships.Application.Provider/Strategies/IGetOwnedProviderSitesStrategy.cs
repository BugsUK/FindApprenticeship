namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface IGetOwnedProviderSitesStrategy
    {
        IEnumerable<ProviderSite> GetOwnedProviderSites(int providerId);
    }
}