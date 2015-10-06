namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Application.Organisation;
    using Configuration;
    using Dapper;
    using Domain.Interfaces.Configuration;
    using Provider = Domain.Entities.Providers.Provider;
    using ProviderSite = Domain.Entities.Providers.ProviderSite;

    public class LegacyProviderProvider : ILegacyProviderProvider
    {
        private readonly string _connectionString;

        public LegacyProviderProvider(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public Provider GetProvider(string ukprn)
        {
            const string sql = @"SELECT * FROM dbo.Provider WHERE UKPRN = @Ukprn;";

            Models.Provider legacyProvider;

            using (var connection = GetConnection())
            {
                legacyProvider = connection.Query<Models.Provider>(sql, new {Ukprn = ukprn}).SingleOrDefault();
            }

            return GetProvider(legacyProvider);
        }

        private static Provider GetProvider(Models.Provider legacyProvider)
        {
            if (legacyProvider == null)
            {
                return null;
            }

            var provider = new Provider
            {
                Ukprn = legacyProvider.UKPRN.ToString(),
                Name = legacyProvider.FullName
            };

            return provider;
        }

        public IEnumerable<ProviderSite> GetProviderSites(string ukprn)
        {
            const string sql = @"SELECT p.UKPRN, ps.* FROM dbo.Provider AS p JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId WHERE p.UKPRN = @Ukprn";

            IList<Models.ProviderSite> legacyProviderSites;

            using (var connection = GetConnection())
            {
                legacyProviderSites = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn }).ToList();
            }

            return legacyProviderSites.Select(GetProviderSite);
        }

        private static ProviderSite GetProviderSite(Models.ProviderSite legacyProviderSite)
        {
            if (legacyProviderSite == null)
            {
                return null;
            }

            var providerSite = new ProviderSite
            {
                Ukprn = legacyProviderSite.UKPRN.ToString(),
                Ern = legacyProviderSite.EDSURN.ToString(),
                Name = legacyProviderSite.FullName,
                //EmailAddress = legacyProviderSite.ContactDetailsForEmployer,
            };

            return providerSite;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}