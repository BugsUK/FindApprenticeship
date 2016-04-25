namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using MongoDB.Driver;
    using Repository.Mongo;
    using Repository.Sql;
    using SFA.Infrastructure.Interfaces;

    public class CandidateMigrationProcessor : IMigrationProcessor
    {
        private readonly ICandidateMappers _candidateMappers;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly IGetOpenConnection _targetDatabase;
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;

        private readonly CandidateRepository _candidateRepository;
        private readonly PersonRepository _personRepository;
        private readonly CandidateUserRepository _candidateUserRepository;
        private readonly UserRepository _userRepository;
        private readonly SyncRepository _syncRepository;

        private readonly ITableSpec _candidateTable = new CandidateTable();
        private readonly ITableSpec _personTable = new PersonTable();

        public CandidateMigrationProcessor(ICandidateMappers candidateMappers, SyncRepository syncRepository, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _candidateMappers = candidateMappers;
            _syncRepository = syncRepository;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _configurationService = configurationService;
            _logService = logService;

            _candidateRepository = new CandidateRepository(targetDatabase);
            _personRepository = new PersonRepository(targetDatabase);
            _candidateUserRepository = new CandidateUserRepository(configurationService, _logService);
            _userRepository = new UserRepository(configurationService, logService);
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
            ProcessCandidates(candidateUsers, expectedCount, cancellationToken);
        }

        private void ExecuteIncrementalSync(SyncParams syncParams, CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on candidates collection with LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");

            //Inserts
            _logService.Info("Processing new candidates");
            var expectedCreatedCount = _candidateUserRepository.GetCandidatesCreatedSinceCount(syncParams.CandidateLastCreatedDate, cancellationToken).Result;
            var createdCursor = _candidateUserRepository.GetAllCandidateUsersCreatedSince(syncParams.CandidateLastCreatedDate, cancellationToken).Result;
            ProcessCandidates(createdCursor, expectedCreatedCount, cancellationToken);
            _logService.Info("Completed processing new candidates");

            //Updates
            _logService.Info("Processing updated candidates");
            var expectedUpdatedCount = _candidateUserRepository.GetCandidatesUpdatedSinceCount(syncParams.CandidateLastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _candidateUserRepository.GetAllCandidateUsersUpdatedSince(syncParams.CandidateLastUpdatedDate, cancellationToken).Result;
            ProcessCandidates(updatedCursor, expectedUpdatedCount, cancellationToken);
            _logService.Info("Completed processing updated candidates");
        }

        private void ProcessCandidates(IAsyncCursor<Entities.Mongo.Candidate> cursor, long expectedCount, CancellationToken cancellationToken)
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

                var candidateIds = _candidateRepository.GetCandidateIdsByGuid(candidateUsers.Select(c => c.Candidate.Id));
                var personIds = _personRepository.GetPersonIdsByEmails(candidateUsers.Select(c => c.Candidate.RegistrationDetails.EmailAddress));
                var candidatePersons = candidateUsers.Where(c => c.User.Status >= 20 && c.User.Status != 999).Select(c => _candidateMappers.MapCandidatePerson(c, candidateIds, personIds)).ToList();
                
                count += candidatePersons.Count;
                _logService.Info($"Processing {candidatePersons.Count} active candidates");
                BulkUpsert(candidatePersons, candidateIds);

                var syncParams = _syncRepository.GetSyncParams();
                syncParams.CandidateLastCreatedDate = maxDateCreated > syncParams.CandidateLastCreatedDate ? maxDateCreated : syncParams.CandidateLastCreatedDate;
                syncParams.CandidateLastUpdatedDate = maxDateUpdated > syncParams.CandidateLastUpdatedDate ? maxDateUpdated : syncParams.CandidateLastUpdatedDate;
                _syncRepository.SetCandidateSyncParams(syncParams);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {candidatePersons.Count} candidates and {count} candidates out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");
            }
        }

        private void BulkUpsert(IList<CandidatePerson> candidatePersons, IDictionary<Guid, int> candidateIds)
        {
            //Have to do these one at a time as need to get the id for the inserted person records
            foreach (var candidatePerson in candidatePersons.Where(c => c.Person.PersonId == 0))
            {
                //Insert any new person records to match with candidate records
                var personId = _targetDatabase.Insert(candidatePerson.Person);
                candidatePerson.Candidate.PersonId = (int)personId;
            }

            //Update any existing person records
            _genericSyncRespository.BulkUpdate(_personTable, candidatePersons.Where(c => c.Person.PersonId != 0).Select(c => _candidateMappers.MapPersonDictionary(c.Person)));

            //Bulk insert any candidates with valid ids that are not already in the database
            _genericSyncRespository.BulkInsert(_candidateTable, candidatePersons.Where(c => c.Candidate.CandidateId != 0 && !candidateIds.ContainsKey(c.Candidate.CandidateGuid)).Select(c => _candidateMappers.MapCandidateDictionary(c.Candidate)));

            //Now insert any remaining candidates one at a time
            foreach (var candidate in candidatePersons.Where(c => c.Candidate.CandidateId == 0).Select(c => c.Candidate))
            {
                _targetDatabase.Insert(candidate);
            }

            //Finally, update existing candidates
            _genericSyncRespository.BulkUpdate(_candidateTable, candidatePersons.Where(c => c.Candidate.CandidateId != 0 && candidateIds.ContainsKey(c.Candidate.CandidateGuid)).Select(c => _candidateMappers.MapCandidateDictionary(c.Candidate)));
        }
    }
}