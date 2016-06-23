namespace SFA.WebProxy.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Configuration;
    using Dapper;
    using Models;

    public class SqlServerWebProxyUserRepository : IWebProxyUserRepository
    {
        private readonly IConfiguration _configuration;

        public SqlServerWebProxyUserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WebProxyConsumer Get(Guid externalSystemId)
        {
            /*
                CREATE SCHEMA WebProxy

                CREATE TABLE WebProxy.WebProxyConsumer
                (
                    ExternalSystemId                    UNIQUEIDENTIFIER PRIMARY KEY,
                    WebProxyConsumerId                  INT,
                    ShortDescription                    VARCHAR(MAX),
                    FullDescription                     VARCHAR(MAX),
                    RouteToCompatabilityWebServiceRegex VARCHAR(MAX)
                )
            */

            const string sql = @"SELECT * FROM WebProxy.WebProxyConsumer WHERE ExternalSystemId = @ExternalSystemId";

            WebProxyConsumer webProxyUser;

            using (var connection = GetConnection())
            {
                webProxyUser = connection.Query<WebProxyConsumer>(sql, new { ExternalSystemId = externalSystemId }).SingleOrDefault();
            }

            return webProxyUser ?? WebProxyConsumer.WebProxyConsumerNotFound;
        }

        private IDbConnection GetConnection()
        {
            var conn = new SqlConnection(_configuration.SqlServerConnectionString);
            conn.Open();
            return conn;
        }
    }
}