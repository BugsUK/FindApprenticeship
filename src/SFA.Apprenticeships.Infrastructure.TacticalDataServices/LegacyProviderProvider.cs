namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Application.Organisation;
    using Configuration;
    using Dapper;
    using Domain.Entities.Locations;
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
            const string sql = @"SELECT p.UKPRN, ps.* FROM dbo.Provider AS p JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId WHERE p.UKPRN = @Ukprn AND ps.TrainingProviderStatusTypeId = 1";

            IList<Models.ProviderSite> legacyProviderSites;

            using (var connection = GetConnection())
            {
                legacyProviderSites = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn }).ToList();
            }

            return legacyProviderSites.Select(GetProviderSite);
        }

        public ProviderSite GetProviderSite(string ukprn, string ern)
        {
            const string sql = @"SELECT p.UKPRN, ps.* FROM dbo.Provider AS p JOIN dbo.ProviderSiteRelationship AS psr ON p.ProviderID = psr.ProviderID JOIN ProviderSite AS ps ON psr.ProviderSiteID = ps.ProviderSiteId WHERE p.UKPRN = @Ukprn AND ps.EDSURN = @Ern AND ps.TrainingProviderStatusTypeId = 1";

            Models.ProviderSite legacyProviderSite;

            using (var connection = GetConnection())
            {
                legacyProviderSite = connection.Query<Models.ProviderSite>(sql, new { Ukprn = ukprn, Ern = ern }).SingleOrDefault();
            }

            return GetProviderSite(legacyProviderSite);
        }

        private static ProviderSite GetProviderSite(Models.ProviderSite legacyProviderSite)
        {
            if (legacyProviderSite == null)
            {
                return null;
            }

            var address = new Address
            {
                AddressLine1 = legacyProviderSite.AddressLine1,
                AddressLine2 = legacyProviderSite.AddressLine2,
                AddressLine3 = legacyProviderSite.AddressLine3,
                AddressLine4 = legacyProviderSite.Town,
                Postcode = legacyProviderSite.PostCode,
                GeoPoint = new GeoPoint
                {
                    Latitude = legacyProviderSite.Latitude,
                    Longitude = legacyProviderSite.Longitude
                },
                //Uprn = 
            };

            var providerSite = new ProviderSite
            {
                Ukprn = legacyProviderSite.UKPRN.ToString(),
                Ern = legacyProviderSite.EDSURN.ToString(),
                Name = legacyProviderSite.FullName,
                EmployerDescription = legacyProviderSite.EmployerDescription,
                CandidateDescription = legacyProviderSite.CandidateDescription,
                ContactDetailsForEmployer = legacyProviderSite.ContactDetailsForEmployer,
                ContactDetailsForCandidate = legacyProviderSite.ContactDetailsForCandidate,
                Address = address
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