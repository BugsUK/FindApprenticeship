namespace SFA.Apprenticeships.Infrastructure.Repositories.Reference
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Configuration;
    using Dapper;
    using Domain.Entities.Reference;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;

    //TODO: Remove once sql project is complete
    public class TacticalReferenceRepository : IReferenceRepository
    {
        private readonly string _connectionString;

        public TacticalReferenceRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<TacticalDataServivcesConfiguration>();
            _connectionString = config.AvSqlReferenceConnectionString;
        }

        public IList<County> GetCounties()
        {
            const string sql = @"SELECT * FROM County WHERE CountyId <> 0 ORDER BY FullName";

            IList<County> counties;

            using (var connection = GetConnection())
            {
                counties = connection.Query<County>(sql).ToList();
            }

            return counties;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}