namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IProviderSiteReadRepository
    {
        ProviderSite GetById(int providerSiteId);

        ProviderSite GetByEdsUrn(string edsUrn);

        IEnumerable<ProviderSite> GetByUkprn(string ukprn);

        IEnumerable<ProviderSite> GetByIds(IEnumerable<int> providerSiteIds);
    }

    public interface IProviderSiteWriteRepository
    {
        ProviderSite Update(ProviderSite providerSite);
    }
}
