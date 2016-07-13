namespace SetApplicationStatus.Console.Repositories.Sql
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Dapper;
    using Entities;
    using Entities.Sql;

    public class ApplicationRepository
    {
        private readonly IDbConnection _connection;

        public ApplicationRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IEnumerable<Application> GetApplicationIdsByUkprn(string ukprn)
        {
            const string sql = @"
                SELECT
                    a.ApplicationId,
                    a.ApplicationStatusTypeId AS Status
                FROM dbo.Application a
                INNER JOIN dbo.Vacancy v
                ON v.VacancyId = a.VacancyId
                INNER JOIN dbo.Provider p
                ON p.ProviderId = v.ContractOwnerId
                WHERE p.UKPRN = @ukprn
                ";

            return _connection.Query<Application>(sql, new
            {
                ukprn
            });
        }
   }
}
