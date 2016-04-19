namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using Dapper;
    using Entities.Mongo;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using MongoDB.Driver;
    using Repository.Mongo;
    using Repository.Sql;
    using SFA.Infrastructure.Interfaces;

    public class VacancyApplicationsMigrationProcessor : IMigrationProcessor
    {
        private readonly IVacancyApplicationsUpdater _vacancyApplicationsUpdater;
        private readonly IApplicationMappers _applicationMappers;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly IGetOpenConnection _targetDatabase;
        private readonly ILogService _logService;
        
        private readonly VacancyRepository _vacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly VacancyApplicationsRepository _vacancyApplicationsRepository;

        private readonly ITableSpec _applicationTable = new ApplicationTable();

        public VacancyApplicationsMigrationProcessor(IVacancyApplicationsUpdater vacancyApplicationsUpdater, IApplicationMappers applicationMappers, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _vacancyApplicationsUpdater = vacancyApplicationsUpdater;
            _applicationMappers = applicationMappers;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _logService = logService;

            _vacancyRepository = new VacancyRepository(targetDatabase);
            _candidateRepository = new CandidateRepository(configurationService, _logService);
            _vacancyApplicationsRepository = new VacancyApplicationsRepository(_vacancyApplicationsUpdater.CollectionName, configurationService, logService);
        }

        public void Process(CancellationToken cancellationToken)
        {
            if (_vacancyApplicationsUpdater.IsValidForIncrementalSync)
            {
                ExecuteIncrementalSync(cancellationToken);
            }
            else
            {
                ExecuteFullSync(cancellationToken);
            }
        }

        private void ExecuteFullSync(CancellationToken cancellationToken)
        {
            _logService.Warn($"ExecuteFullSync on {_vacancyApplicationsUpdater.CollectionName} collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            //_genericSyncRespository.DeleteAll(_applicationTable);

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            IDictionary<Guid, Candidate> candidates;
            if (_vacancyApplicationsUpdater.LoadAllCandidatesBeforeProcessing)
            {
                _logService.Info("Loading candidates");
                candidates = _candidateRepository.GetAllCandidatesAsync(cancellationToken).Result;
                _logService.Info($"Completed loading {candidates.Count} candidates");
            }
            else
            {
                candidates = new Dictionary<Guid, Candidate>();
            }

            var expectedCount = _vacancyApplicationsRepository.GetVacancyApplicationsCount(cancellationToken).Result;
            var cursor = _vacancyApplicationsRepository.GetAllVacancyApplications(cancellationToken).Result;
            ProcessApplications(cursor, expectedCount, vacancyIds, candidates, BulkInsert, cancellationToken);
        }

        private void ExecuteIncrementalSync(CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on {_vacancyApplicationsUpdater.CollectionName} collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            var candidates = new Dictionary<Guid, Candidate>();

            //Inserts
            _logService.Info($"Processing new {_vacancyApplicationsUpdater.CollectionName}");
            var expectedCreatedCount = _vacancyApplicationsRepository.GetVacancyApplicationsCreatedSinceCount(_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate, cancellationToken).Result;
            var createdCursor = _vacancyApplicationsRepository.GetAllVacancyApplicationsCreatedSince(_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate, cancellationToken).Result;
            ProcessApplications(createdCursor, expectedCreatedCount, vacancyIds, candidates, BulkInsert, cancellationToken);
            _logService.Info($"Completed processing new {_vacancyApplicationsUpdater.CollectionName}");

            //Updates
            _logService.Info($"Processing updated {_vacancyApplicationsUpdater.CollectionName}");
            var expectedUpdatedCount = _vacancyApplicationsRepository.GetVacancyApplicationsUpdatedSinceCount(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _vacancyApplicationsRepository.GetAllVacancyApplicationsUpdatedSince(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            ProcessApplications(updatedCursor, expectedUpdatedCount, vacancyIds, candidates, BulkUpdate, cancellationToken);
            _logService.Info($"Completed processing updated {_vacancyApplicationsUpdater.CollectionName}");
        }

        private void BulkInsert(IList<IDictionary<string, object>> applications)
        {
            _genericSyncRespository.BulkInsert(_applicationTable, applications);
        }

        private void BulkUpdate(IList<IDictionary<string, object>> applications)
        {
            //Updates can edit the ApplicationId value so delete the existing records and recreate
            BulkDelete(applications);

            //Then create the new ones
            _genericSyncRespository.BulkInsert(_applicationTable, applications);
        }

        private void BulkDelete(IList<IDictionary<string, object>> applications)
        {
            using (var connection = (SqlConnection) _targetDatabase.GetOpenConnection())
            {
                //Delete via guids first
                connection.Execute($@"DELETE FROM {_applicationTable.Name} WHERE ApplicationGuid IN @applicationGuids", new { applicationGuids = applications.Select(a => (Guid)a["ApplicationGuid"]) });
                foreach (var application in applications)
                {
                    //Haven't identified why but there are cases where we end up with duplicate applications that aren't identified correctly by their guid. This deletes them
                    connection.Execute(
                        $@"DELETE FROM {_applicationTable.Name} WHERE VacancyId = @vacancyId AND CandidateId = @candidateId",
                        new {vacancyId = application["VacancyId"], candidateId = application["CandidateId"]});
                }
            }
        }

        private void ProcessApplications(IAsyncCursor<VacancyApplication> cursor, long expectedCount, HashSet<int> vacancyIds, IDictionary<Guid, Candidate> candidates, Action<IList<IDictionary<string, object>>> bulkAction, CancellationToken cancellationToken)
        {
            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} {_vacancyApplicationsUpdater.CollectionName}");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                LoadCandidates(candidates, batch, cancellationToken);

                var applications = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidates.ContainsKey(a.CandidateId)).Select(a => _applicationMappers.MapApplicationDictionary(a, candidates[a.CandidateId])).ToList();
                count += applications.Count;
                _logService.Info($"Processing {applications.Count} {_vacancyApplicationsUpdater.CollectionName}");
                try
                {
                    bulkAction(applications);
                }
                catch (Exception ex)
                {
                    _logService.Error($"Error while processing {_vacancyApplicationsUpdater.CollectionName}. Trying a delete then retry", ex);
                    //Propagate exception back up the stack rather than silently continuing
                    BulkDelete(applications);
                    bulkAction(applications);
                }

                _vacancyApplicationsUpdater.UpdateSyncDates(maxDateCreated, maxDateUpdated);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {applications.Count} {_vacancyApplicationsUpdater.CollectionName} and {count} {_vacancyApplicationsUpdater.CollectionName} out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");
            }
        }

        private void LoadCandidates(IDictionary<Guid, Candidate> candidates, IEnumerable<VacancyApplication> vacancyApplications, CancellationToken cancellationToken)
        {
            var candidateIds = vacancyApplications.Select(a => a.CandidateId).Except(candidates.Keys).ToArray();
            if (candidateIds.Any())
            {
                _logService.Info($"Loading {candidateIds.Length} candidates");
                var candidatesCursor = _candidateRepository.GetCandidatesByIds(candidateIds, cancellationToken).Result;
                while (candidatesCursor.MoveNextAsync(cancellationToken).Result)
                {
                    var candidatesBatch = candidatesCursor.Current.ToList();
                    foreach (var candidate in candidatesBatch)
                    {
                        candidates[candidate.Id] = candidate;
                    }
                    _logService.Info($"Completed loading batch of {candidatesBatch.Count} candidates");
                }
            }
        }
    }
}