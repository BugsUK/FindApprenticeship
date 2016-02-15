namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;

    using Avms = SFA.Apprenticeships.Avms.Domain.Entities;
    using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Common; // TODO: This stuff should be in a separate project, should not be forced to include Domain.Entities/Interfaces and should be sharable with DAS

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
