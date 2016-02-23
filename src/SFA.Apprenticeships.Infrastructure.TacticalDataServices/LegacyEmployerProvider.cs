namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices
{
    using System.Collections.Generic;
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

        public Employer GetEmployer(string edsUrn)
        {
            const string sql = @"SELECT e.* FROM dbo.Employer AS e WHERE e.EdsUrn = @EdsUrn AND e.EmployerStatusTypeId = 1";

            Models.Employer legacyEmployer;

            using (var connection = GetConnection())
            {
                legacyEmployer = connection.Query<Models.Employer>(sql, new { EdsUrn = edsUrn }).SingleOrDefault();
            }

            return GetEmployer(legacyEmployer);
        }

        public IEnumerable<Employer> GetEmployersByIds(IEnumerable<int> employerIds)
        {
            const string sql = @"SELECT e.* FROM dbo.Employer AS e WHERE e.EmployerId IN @Ids AND e.EmployerStatusTypeId = 1";

            IEnumerable<Models.Employer> legacyEmployers;

            using (var connection = GetConnection())
            {
                legacyEmployers = connection.Query<Models.Employer>(sql, new { Ids = employerIds });
            }

            return legacyEmployers.Select(GetEmployer).ToList();
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
                EmployerId = legacyEmployer.EmployerId,
                EdsUrn = legacyEmployer.EdsUrn.ToString(),
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