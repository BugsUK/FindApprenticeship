namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Providers;

    public interface ILegacyProviderProvider
    {
        Provider GetProvider(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string ern);
    }
}