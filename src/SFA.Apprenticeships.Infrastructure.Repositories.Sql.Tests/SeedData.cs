namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests
{
    using Domain.Entities.Raa.Parties;
    using AgencyUser = Sql.Schemas.UserProfile.Entities.AgencyUser;
    using ProviderUser = Sql.Schemas.Provider.Entities.ProviderUser;

    // NOTE: test data here corresponds to ./Scripts/*.sql
    public class SeedData
    {
        public static class Providers
        {
            public static readonly Provider HopwoodHallCollege = new Provider
            {
                ProviderId = 10000001,
                Ukprn = "10000002"
            };
        }

        public static class ProviderSites
        {
            public static readonly ProviderSite HopwoodCampus = new ProviderSite
            {
                ProviderSiteId = 20000001,
                EdsUrn = "21000002"
            };

            public static readonly ProviderSite HopwoodHallCollege = new ProviderSite
            {
                ProviderSiteId = 20000002
            };
        }

        public static class ProviderUsers
        {
            public static readonly ProviderUser JaneDoe = new ProviderUser
            {
                Username = "jane.doe@example.com"
            };
        }

        public static class AgencyUsers
        {
            public static readonly AgencyUser JaneAgency = new AgencyUser
            {
                Username = "jane.agency@sfa.bis.gov.uk"
            };
        }

        public static class Employers
        {
            public static readonly Employer AcmeCorp = new Employer
            {
                EmployerId = 50000001,
                EdsUrn = "21000002"
            };

            public static readonly Employer AwesomeInc = new Employer
            {
                EmployerId = 50000002
            };
        }
    }
}
