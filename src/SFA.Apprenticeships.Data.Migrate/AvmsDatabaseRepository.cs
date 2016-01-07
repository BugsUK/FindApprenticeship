namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;

    using Avms = SFA.Apprenticeships.Avms.Domain.Entities;
    using Infrastructure.Sql;

    public class AvmsDatabaseRespository : IAvmsRepository
    {
        public IGetOpenConnection _getOpenConnection;

        public AvmsDatabaseRespository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IEnumerable<Avms.Vacancy> GetAllVacancies()
        {
            return _getOpenConnection.QueryProgressive<Avms.Vacancy>(@"
SELECT *
FROM   Vacancy
");
        }

    }
}
