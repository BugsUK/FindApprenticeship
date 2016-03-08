
namespace SFA.Apprenticeships.Data.Migrate
{
    using Infrastructure.Repositories.Sql.Common;
    using System;
    using Dapper;
    using System.Collections.Generic;
    using SFA.Infrastructure.Interfaces;
    using System.Linq;

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

                _vacanciesOwnedByUs = new HashSet<int>(_targetDatabase.Query<int>("SELECT VacancyId FROM Vacancy WHERE EditedInRaa = 1"));
                _vacanciesOwnedByUsLastFullyUpdatedUtc = DateTime.UtcNow;
            }

            return _vacanciesOwnedByUs.Contains(vacancyId);
        }

        public bool IsVacancyOwnerRelationshipOwnedByTargetDatabase(int vacancyOwnerRelationshipId)
        {
            if (DateTime.UtcNow > _vacancyOwnerRelationshipsOwnedByUsLastFullyUpdatedUtc + _cacheTime)
            {
                _log.Debug("Refreshing _vacancyOwnerRelationshipsOwnedByUs");

                _vacancyOwnerRelationshipsOwnedByUs = new HashSet<int>(_targetDatabase.Query<int>("SELECT VacancyOwnerRelationshipId FROM VacancyOwnerRelationship WHERE EditedInRaa = 1"));
                _vacancyOwnerRelationshipsOwnedByUsLastFullyUpdatedUtc = DateTime.UtcNow;
            }

            return _vacancyOwnerRelationshipsOwnedByUs.Contains(vacancyOwnerRelationshipId);
        }

        public AnonDetail GetAnonymousDetails(int id)
        {
            // male,England/Wales,Mr.,Luke,C,Cox,"80 Winchester Rd",MESSING,,"CO5 5GX","United Kingdom",LukeCox@armyspy.com,"078 6922 5444",Nicholls,2/22/1990
            return new AnonDetail()
            {
                Gender = "male",
                Title = "Mr.",
                GivenName = "Luke",
                MiddleInitial = "C",
                Surname = "Cox",
                StreetAddress = "80 Winchester Rd",
                City = "MESSING",
                ZipCode = "CO5 5GX",
                CountryFull = "United Kingdom",
                EmailAddress = "LukeCox@armyspy.com",
                TelephoneNumber = "078 6922 5444",
                MothersMaiden = "Nicholls",
                Birthday = "2/22/1990"
            };

            /*
            return _fakeNameDatabase.QueryCachedDictionary<int, AnonDetail>(TimeSpan.FromHours(1),
                "SELECT * FROM FakeNameGeneratorDetails")
            */
        }

        public IDictionary<string,int> GetPersonTitleTypeIdsByTitleFullName()
        {
            return new Dictionary<string, int>()
            {
                { "Mr", 1 },
                { "Ms", 2 },
                { "Miss", 3 },
                { "Mrs", 4 },
                { "Master", 5 },

            };

            /*
            return _targetDatabase.QueryCachedDictionary<string, int>(TimeSpan.FromHours(1),
                "SELECT FullName, PersonTitleTypeId FROM PersonTitleType WHERE PersonTitleTypeId NOT IN (0, 6)")
                .ToDictionary(v => (string)v.FullName, v => (int)v.PersonTitleTypeId);
            */
        }
    }
}
