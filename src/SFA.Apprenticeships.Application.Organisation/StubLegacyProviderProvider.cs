namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Providers;

    public class StubLegacyProviderProvider : ILegacyProviderProvider
    {
        //TODO: Replace with real implementation

        public Provider GetProvider(string ukprn)
        {
            return new Provider
            {
                Ukprn = ukprn,
                Name = "Provider " + ukprn
            };
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            if (ukprn == "onesite")
            {
                return new List<ProviderSite>
                {
                    CreateProviderSite(ukprn, "01")
                };
            }

            return new List<ProviderSite>
            {
                CreateProviderSite(ukprn, "01"),
                CreateProviderSite(ukprn, "02"),
                CreateProviderSite(ukprn, "03")
            };
        }

        private static ProviderSite CreateProviderSite(string ukprn, string siteNumber)
        {
            return new ProviderSite
            {
                Ern = ukprn + siteNumber,
                Ukprn = ukprn,
                Name = string.Format("Provider {0}'s Site {1}", ukprn, siteNumber),
                EmailAddress = string.Format("site{1}@provider{0}.com", ukprn, siteNumber),
                PhoneNumber = "0123456789"
            };
        }
    }
}