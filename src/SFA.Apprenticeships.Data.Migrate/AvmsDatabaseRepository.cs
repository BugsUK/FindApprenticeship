
namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;

    using System.Data.SqlClient;
    using Avms = SFA.Apprenticeships.Avms.Domain.Entities;
    using System.Data;

    public class AvmsDatabaseRespository : IAvmsRepository
    {
        public Func<IDbConnection> _getOpenConnection;

        public AvmsDatabaseRespository(Func<IDbConnection> getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IEnumerable<Avms.Vacancy> GetAllVacancies()
        {
            using (var conn = _getOpenConnection())
            {
                var results = conn.Query<Avms.Vacancy>(@"
SELECT *
FROM   Vacancy
", buffered: false);

                // Need to keep the database connection open until have finished iterating through the results
                foreach (var row in results)
                {
                    yield return row;
                }
            }
        }

    }
}
