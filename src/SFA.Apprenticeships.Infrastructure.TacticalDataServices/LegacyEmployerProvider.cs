namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Application.Organisation;
    using Configuration;
    using Dapper;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using SFA.Infrastructure.Interfaces;

    public class LegacyEmployerProvider : ILegacyEmployerProvider
    {
        private readonly string _connectionString;

        public LegacyEmployerProvider(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public Employer GetEmployer(int employerId)
        {
            const string sql = @"SELECT e.* FROM dbo.Employer AS e WHERE e.EmployerId = @EmployerId AND e.EmployerStatusTypeId = 1";

            Models.Employer legacyEmployer;

            using (var connection = GetConnection())
            {
                legacyEmployer = connection.Query<Models.Employer>(sql, new { EmployerId = employerId }).SingleOrDefault();
            }

            return GetEmployer(legacyEmployer);
        }

        public Employer GetEmployer(string ern)
        {
            const string sql = @"SELECT e.* FROM dbo.Employer AS e WHERE e.EdsUrn = @Ern AND e.EmployerStatusTypeId = 1";

            Models.Employer legacyEmployer;

            using (var connection = GetConnection())
            {
                legacyEmployer = connection.Query<Models.Employer>(sql, new { Ern = ern }).SingleOrDefault();
            }

            return GetEmployer(legacyEmployer);
        }

        private static Employer GetEmployer(Models.Employer legacyEmployer)
        {
            if (legacyEmployer == null)
            {
                return null;
            }

            var address = new PostalAddress
            {
                AddressLine1 = legacyEmployer.AddressLine1,
                AddressLine2 = legacyEmployer.AddressLine2,
                AddressLine3 = legacyEmployer.AddressLine3,
                AddressLine4 = legacyEmployer.Town,
                Postcode = legacyEmployer.PostCode,
                GeoPoint = new GeoPoint
                {
                    Latitude = legacyEmployer.Latitude,
                    Longitude = legacyEmployer.Longitude
                },
                //Uprn = 
            };

            var employer = new Employer
            {
                EdsErn = legacyEmployer.EdsUrn.ToString(),
                Name = legacyEmployer.FullName,
                Address = address
            };

            return employer;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}