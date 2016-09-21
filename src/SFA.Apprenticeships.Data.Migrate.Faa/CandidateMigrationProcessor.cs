namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Configuration;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using MongoDB.Driver;
    using Repository.Mongo;
    using Repository.Sql;
    using Application.Interfaces;
    using Candidate = Entities.Mongo.Candidate;
    using CandidateSummary = Entities.Sql.CandidateSummary;

    public class CandidateMigrationProcessor : IMigrationProcessor
    {
        private readonly ICandidateMappers _candidateMappers;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly IGetOpenConnection _targetDatabase;
        private readonly ILogService _logService;

        private readonly VacancyRepository _vacancyRepository;
        private readonly LocalAuthorityRepository _localAuthorityRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly SchoolAttendedRepository _schoolAttendedRepository;
        private readonly CandidateHistoryRepository _candidateHistoryRepository;
        private readonly CandidateUserRepository _candidateUserRepository;
        private readonly UserRepository _userRepository;
        private readonly SyncRepository _syncRepository;

        private readonly ITableSpec _candidateTable = new CandidateTable();
        private readonly ITableSpec _personTable = new PersonTable();
        private readonly ITableSpec _schoolsAttendedTable = new SchoolAttendedTable();
        private readonly ITableSpec _candidateHistoryTable = new CandidateHistoryTable();

        private readonly bool _anonymiseData;

        public CandidateMigrationProcessor(ICandidateMappers candidateMappers, SyncRepository syncRepository, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _candidateMappers = candidateMappers;
            _syncRepository = syncRepository;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _logService = logService;

            _vacancyRepository = new VacancyRepository(targetDatabase);
            _localAuthorityRepository = new LocalAuthorityRepository(targetDatabase);
            _candidateRepository = new CandidateRepository(targetDatabase);
            _schoolAttendedRepository = new SchoolAttendedRepository(targetDatabase);
            _candidateHistoryRepository = new CandidateHistoryRepository(targetDatabase);
            _candidateUserRepository = new CandidateUserRepository(configurationService, _logService);
            _userRepository = new UserRepository(configurationService, logService);

            var persistentConfig = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();
            _anonymiseData = persistentConfig.AnonymiseData;
        }

        public void Process(CancellationToken cancellationToken)
        {
            var syncParams = _syncRepository.GetSyncParams();
            if (syncParams.IsValidForCandidateIncrementalSync)
            {
                ExecuteIncrementalSync(syncParams, cancellationToken);
            }
            else
            {
                ExecuteFullSync(syncParams, cancellationToken);
            }
        }

        private void ExecuteFullSync(SyncParams syncParams, CancellationToken cancellationToken)
        {
            _logService.Warn($"ExecuteFullSync on candidates collection with LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");

            //TODO: This delete would have to be done outside of this class as it affects traineeships and apprenticeships at the same time
            //_genericSyncRespository.DeleteAll(_candidateTable);

            var expectedCount = _candidateUserRepository.GetCandidatesCount(cancellationToken).Result;
            var candidateUsers = _candidateUserRepository.GetAllCandidateUsers(cancellationToken).Result;
            var vacancyLocalAuthorities = _vacancyRepository.GetAllVacancyLocalAuthorities();
            var localAuthorityCountyIds = _localAuthorityRepository.GetLocalAuthorityCountyIds();
            ProcessCandidates(candidateUsers, expectedCount, vacancyLocalAuthorities, localAuthorityCountyIds, SyncType.Full, cancellationToken);
        }

        private void ExecuteIncrementalSync(SyncParams syncParams, CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on candidates collection with LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");

            var vacancyLocalAuthorities = _vacancyRepository.GetAllVacancyLocalAuthorities();
            var localAuthorityCountyIds = _localAuthorityRepository.GetLocalAuthorityCountyIds();

            //Inserts
            _logService.Info("Processing new candidates");
            var expectedCreatedCount = _candidateUserRepository.GetCandidatesCreatedSinceCount(syncParams.CandidateLastCreatedDate, cancellationToken).Result;
            var createdCursor = _candidateUserRepository.GetAllCandidateUsersCreatedSince(syncParams.CandidateLastCreatedDate, cancellationToken).Result;
            ProcessCandidates(createdCursor, expectedCreatedCount, vacancyLocalAuthorities, localAuthorityCountyIds, SyncType.PartialByDateCreated, cancellationToken);
            _logService.Info("Completed processing new candidates");

            //Updates
            _logService.Info("Processing updated candidates");
            var expectedUpdatedCount = _candidateUserRepository.GetCandidatesUpdatedSinceCount(syncParams.CandidateLastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _candidateUserRepository.GetAllCandidateUsersUpdatedSince(syncParams.CandidateLastUpdatedDate, cancellationToken).Result;
            ProcessCandidates(updatedCursor, expectedUpdatedCount, vacancyLocalAuthorities, localAuthorityCountyIds, SyncType.PartialByDateUpdated, cancellationToken);
            _logService.Info("Completed processing updated candidates");
        }

        private void ProcessCandidates(IAsyncCursor<Candidate> cursor, long expectedCount, IDictionary<string, int> vacancyLocalAuthorities, IDictionary<int, int> localAuthorityCountyIds, SyncType syncType, CancellationToken cancellationToken)
        {
            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToDictionary(c => c.Id, c => c);
                if (batch.Count == 0) continue;
                var candidateUsers = new List<CandidateUser>(batch.Count);

                _logService.Info($"Loading {batch.Count} users");
                var usersCursor = _userRepository.GetUsersByIds(batch.Keys, cancellationToken).Result;
                while (usersCursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
                {
                    candidateUsers.AddRange(usersCursor.Current.Select(user => new CandidateUser {Candidate = batch[user.Id], User = user}));
                }

                _logService.Info($"Processing {candidateUsers.Count} candidates");

                var maxDateCreated = candidateUsers.Max(c => c.Candidate.DateCreated);
                var maxDateUpdated = candidateUsers.Max(c => c.Candidate.DateUpdated) ?? DateTime.MinValue;

                var candidateSummaries = _candidateRepository.GetCandidateSummariesByGuid(candidateUsers.Select(c => c.Candidate.Id));
                var schoolAttendedIds = _schoolAttendedRepository.GetSchoolAttendedIdsByCandidateIds(candidateSummaries.Values.Select(cs => cs.CandidateId));
                var candidateHistoryIds = _candidateHistoryRepository.GetCandidateHistoryIdsByCandidateIds(candidateSummaries.Values.Select(cs => cs.CandidateId).Distinct());
                var candidatesWithHistory = candidateUsers.Select(c => _candidateMappers.MapCandidateWithHistory(c, candidateSummaries, vacancyLocalAuthorities, localAuthorityCountyIds, schoolAttendedIds, candidateHistoryIds, _anonymiseData)).Where(c => c != null).ToList();
                
                count += candidatesWithHistory.Count;
                _logService.Info($"Processing {candidatesWithHistory.Count} mapped candidates");
                BulkUpsert(candidatesWithHistory, candidateSummaries);

                var syncParams = _syncRepository.GetSyncParams();
                if (syncType == SyncType.Full)
                {
                    syncParams.CandidateLastCreatedDate = maxDateCreated > syncParams.CandidateLastCreatedDate ? maxDateCreated : syncParams.CandidateLastCreatedDate;
                    //Deliberate as date updated could skip some updates post full sync
                    syncParams.CandidateLastUpdatedDate = syncParams.CandidateLastCreatedDate;
                }
                if (syncType == SyncType.PartialByDateCreated)
                {
                    syncParams.CandidateLastCreatedDate = maxDateCreated > syncParams.CandidateLastCreatedDate ? maxDateCreated : syncParams.CandidateLastCreatedDate;
                }
                if (syncType == SyncType.PartialByDateUpdated)
                {
                    syncParams.CandidateLastUpdatedDate = maxDateUpdated > syncParams.CandidateLastUpdatedDate ? maxDateUpdated : syncParams.CandidateLastUpdatedDate;
                }
                _syncRepository.SetCandidateSyncParams(syncParams);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {candidatesWithHistory.Count} candidates and {count} candidates out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");
            }
        }

        private void BulkUpsert(IList<CandidateWithHistory> candidatesWithHistory, IDictionary<Guid, CandidateSummary> candidateSummaries)
        {
            //Have to do these one at a time as need to get the id for the inserted person records
            foreach (var candidateWithHistory in candidatesWithHistory.Where(c => c.CandidatePerson.Person.PersonId == 0))
            {
                //Insert any new person records to match with candidate records
                var personId = (int)_targetDatabase.Insert(candidateWithHistory.CandidatePerson.Person);
                candidateWithHistory.CandidatePerson.Candidate.PersonId = personId;
            }

            //Update any existing person records
            _genericSyncRespository.BulkUpdate(_personTable, candidatesWithHistory.Where(c => c.CandidatePerson.Person.PersonId != 0).Select(c => _candidateMappers.MapPersonDictionary(c.CandidatePerson.Person)));

            //Bulk insert any candidates with valid ids that are not already in the database
            _genericSyncRespository.BulkInsert(_candidateTable, candidatesWithHistory.Where(c => c.CandidatePerson.Candidate.CandidateId != 0 && !candidateSummaries.ContainsKey(c.CandidatePerson.Candidate.CandidateGuid)).Select(c => _candidateMappers.MapCandidateDictionary(c.CandidatePerson.Candidate)));

            //Now insert any remaining candidates one at a time
            foreach (var candidateWithHistory in candidatesWithHistory.Where(c => c.CandidatePerson.Candidate.CandidateId == 0))
            {
                //Ensure school attended and candidate histories have the correct candidate id
                var candidateId = (int)_targetDatabase.Insert(candidateWithHistory.CandidatePerson.Candidate);
                if (candidateWithHistory.CandidatePerson.SchoolAttended != null)
                {
                    candidateWithHistory.CandidatePerson.SchoolAttended.CandidateId = candidateId;
                }
                foreach (var candidateHistory in candidateWithHistory.CandidateHistory)
                {
                    candidateHistory.CandidateId = candidateId;
                }
            }

            //Finally, update existing candidates
            _genericSyncRespository.BulkUpdate(_candidateTable, candidatesWithHistory.Where(c => c.CandidatePerson.Candidate.CandidateId != 0 && candidateSummaries.ContainsKey(c.CandidatePerson.Candidate.CandidateGuid)).Select(c => _candidateMappers.MapCandidateDictionary(c.CandidatePerson.Candidate)));

            //Insert new schools attended
            var newSchoolsAttended = candidatesWithHistory.Where(a => a.CandidatePerson.SchoolAttended != null && a.CandidatePerson.SchoolAttended.SchoolAttendedId == 0).Select(a => a.CandidatePerson.SchoolAttended);
            _genericSyncRespository.BulkInsert(_schoolsAttendedTable, newSchoolsAttended.Select(sa => sa.MapSchoolAttendedDictionary()));

            //Update existing schools attended
            var existingSchoolsAttended = candidatesWithHistory.Where(a => a.CandidatePerson.SchoolAttended != null && a.CandidatePerson.SchoolAttended.SchoolAttendedId != 0).Select(a => a.CandidatePerson.SchoolAttended);
            _genericSyncRespository.BulkUpdate(_schoolsAttendedTable, existingSchoolsAttended.Select(sa => sa.MapSchoolAttendedDictionary()));

            //Insert new candidate history records
            var newCandidateHistories = candidatesWithHistory.SelectMany(a => a.CandidateHistory).Where(a => a.CandidateHistoryId == 0);
            _genericSyncRespository.BulkInsert(_candidateHistoryTable, newCandidateHistories.Select(ah => ah.MapCandidateHistoryDictionary()));

            //Update existing candidate history records
            var existingCandidateHistories = candidatesWithHistory.SelectMany(a => a.CandidateHistory).Where(a => a.CandidateHistoryId != 0);
            _genericSyncRespository.BulkUpdate(_candidateHistoryTable, existingCandidateHistories.Select(ah => ah.MapCandidateHistoryDictionary()));
        }
    }
}