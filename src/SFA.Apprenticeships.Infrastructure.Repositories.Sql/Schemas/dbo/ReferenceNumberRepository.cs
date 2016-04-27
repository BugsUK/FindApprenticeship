namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Linq;
    using Common;
    using Domain.Interfaces.Repositories;

    public class ReferenceNumberRepository : IReferenceNumberRepository
    {
        private readonly IGetOpenConnection _connection;

        public ReferenceNumberRepository(IGetOpenConnection connection)
        {
            _connection = connection;
        }

        public int GetNextVacancyReferenceNumber()
        {
            const string sql = "SELECT NEXT VALUE FOR dbo.VacancyReferenceNumberSequence";

            return _connection.Query<int>(sql).Single();
        }

        public int GetNextLegacyApplicationId()
        {
            const string sql = "SELECT NEXT VALUE FOR dbo.LegacyApplicationIdSequence";

            return _connection.Query<int>(sql).Single();
        }
    }
}
