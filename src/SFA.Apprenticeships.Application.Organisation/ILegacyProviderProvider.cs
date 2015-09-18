namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Providers;

    public interface ILegacyProviderProvider
    {
        //Real implementation -	Provider and provider site data will come from the “snapshot” of data mentioned below (see the TBCs). This will be hosted on Azure in either a SQL Azure database (not VM) or Azure table storage (keyed by UKPRN) or Mongo

        Provider GetProvider(string ukprn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);
    }
}