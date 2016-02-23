
namespace SFA.Apprenticeships.Data.Migrate
{
    using Infrastructure.Repositories.Sql.Common;
    using System;
    using Dapper;
    using System.Collections.Generic;
    using SFA.Infrastructure.Interfaces;

    public class AvmsSyncRespository : IAvmsSyncRespository
    {
        //private IGetOpenConnection _sourceDatabase;
        private IGetOpenConnection _targetDatabase;
        private ILogService _log;
        private TimeSpan _cacheTime = new TimeSpan(0, 1, 0);

        private ISet<int> _vacanciesOwnedByUs;
        private DateTime _vacanciesOwnedByUsLastFullyUpdatedUtc;

        private ISet<int> _vacancyOwnerRelationshipsOwnedByUs;
        private DateTime _vacancyOwnerRelationshipsOwnedByUsLastFullyUpdatedUtc;

        public AvmsSyncRespository(ILogService log, IGetOpenConnection sourceDatabase, IGetOpenConnection targetDatabase)
        {
            _log = log;
            //_sourceDatabase = sourceDatabase;
            _targetDatabase = targetDatabase;
        }

        public bool IsVacancyOwnedByTargetDatabase(int vacancyId)
        {
            // Another implementation of this would be to add to a table with an incrementing primary key + VacancyId containing only records that are owned by us
            // Upon editing a record we would add a record to this table if it didn't already exists.
            // The update would look for records with a primary key higher than the last one, which could be done every second without impact.

            if (DateTime.UtcNow > _vacanciesOwnedByUsLastFullyUpdatedUtc + _cacheTime)
            {
                _log.Debug("Refreshing _vacanciesOwnedByUs");
                /*
                ALTER TABLE Vacancy ADD OwnedByUs BIT NOT NULL DEFAULT 0 -- 18 seconds

                CREATE NONCLUSTERED INDEX [idx_Vacancy_OwnedByUs] ON [dbo].[Vacancy]
                (
	                [OwnedByUs] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                GO
                */

                _vacanciesOwnedByUs = new HashSet<int>(_targetDatabase.Query<int>("SELECT VacancyId FROM Vacancy WHERE OwnedByUs = 1"));
                _vacanciesOwnedByUsLastFullyUpdatedUtc = DateTime.UtcNow;
            }

            return _vacanciesOwnedByUs.Contains(vacancyId);
        }

        public bool IsVacancyOwnerRelationshipOwnedByTargetDatabase(int vacancyOwnerRelationshipId)
        {
            if (DateTime.UtcNow > _vacancyOwnerRelationshipsOwnedByUsLastFullyUpdatedUtc + _cacheTime)
            {
                _log.Debug("Refreshing _vacancyOwnerRelationshipsOwnedByUs");
                /*
                ALTER TABLE VacancyOwnerRelationship ADD OwnedByUs BIT NOT NULL DEFAULT 0 -- 18 seconds

                CREATE NONCLUSTERED INDEX [idx_VacancyOwnerRelationship_OwnedByUs] ON [dbo].[VacancyOwnerRelationship]
                (
	                [OwnedByUs] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
                GO
                */

                _vacancyOwnerRelationshipsOwnedByUs = new HashSet<int>(_targetDatabase.Query<int>("SELECT VacancyOwnerRelationshipId FROM VacancyOwnerRelationship WHERE OwnedByUs = 1"));
                _vacancyOwnerRelationshipsOwnedByUsLastFullyUpdatedUtc = DateTime.UtcNow;
            }

            return _vacancyOwnerRelationshipsOwnedByUs.Contains(vacancyOwnerRelationshipId);
        }
    }
}
