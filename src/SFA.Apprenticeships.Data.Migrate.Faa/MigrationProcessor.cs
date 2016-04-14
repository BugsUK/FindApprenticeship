namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities.Mongo;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using MongoDB.Driver;
    using Repository.Mongo;
    using Repository.Sql;
    using SFA.Infrastructure.Interfaces;

    public class MigrationProcessor
    {
        private readonly ILogService _logService;

        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly VacancyRepository _vacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly ApprenticeshipApplicationsRepository _apprenticeshipApplicationsRepository;
        private readonly SyncRepository _syncRepository;

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            _logService.Info("Initialisation");

            var persistentConfig = configurationService.Get<MigrateFromAvmsConfiguration>();

            var sourceDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.TargetConnectionString);
            _genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            _candidateRepository = new CandidateRepository(configurationService, _logService);
            _apprenticeshipApplicationsRepository = new ApprenticeshipApplicationsRepository(configurationService);
            _syncRepository = new SyncRepository(targetDatabase);
            _vacancyRepository = new VacancyRepository(targetDatabase);
        }

        public void Execute(CancellationToken cancellationToken)
        {
            _logService.Info("Execute Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var lastCreatedDate = _syncRepository.GetApplicationLastCreatedDate();
                    var lastUpdatedDate = _syncRepository.GetApplicationLastUpdatedDate();

                    if (lastCreatedDate == null || lastUpdatedDate == null)
                    {
                        ExecuteFullSync(cancellationToken);
                    }
                    else
                    {
                        ExecutePartialSync(new DateTime(lastCreatedDate.Value.Ticks, DateTimeKind.Utc), new DateTime(lastUpdatedDate.Value.Ticks, DateTimeKind.Utc), cancellationToken);
                    }
                }
                catch (FatalException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logService.Error("Error occurred. Sleeping before trying again", ex);
                }

                Thread.Sleep(10000);
            }

            _logService.Info("DoAll Cancelled");
        }

        private void ExecuteFullSync(CancellationToken cancellationToken)
        {
            _logService.Warn("ExecuteFullSync");

            var applicationTable = new ApplicationTable();
            _genericSyncRespository.DeleteAll(applicationTable);

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            _logService.Info("Loading candidates");
            var candidates = _candidateRepository.GetAllCandidatesAsync(cancellationToken).Result;
            _logService.Info($"Completed loading {candidates.Count} candidates");

            var expectedCount = _apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsCount(cancellationToken).Result;
            var count = 0;

            var lastCreatedDate = DateTime.MinValue;
            var lastUpdatedDate = DateTime.MinValue;

            var cursor = _apprenticeshipApplicationsRepository.GetAllApprenticeshipApplications(cancellationToken).Result;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} Applications");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                var applications = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidates.ContainsKey(a.CandidateId)).Select(a => a.ToApplicationDictionary(candidates[a.CandidateId])).ToList();
                count += applications.Count;
                _logService.Info($"Inserting {applications.Count} Applications");
                try
                {
                    _genericSyncRespository.BulkInsert(applicationTable, applications);
                }
                catch (Exception ex)
                {
                    _logService.Error("Error while inserting applications", ex);
                }

                lastCreatedDate = maxDateCreated > lastCreatedDate ? maxDateCreated : lastCreatedDate;
                lastUpdatedDate = maxDateUpdated > lastUpdatedDate ? maxDateUpdated : lastUpdatedDate;
                _syncRepository.SetApplicationLastCreatedDate(lastCreatedDate);
                _syncRepository.SetApplicationLastUpdatedDate(lastUpdatedDate);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Inserted {applications.Count} Applications and {count} Applications out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. lastCreatedDate: {lastCreatedDate} lastUpdatedDate: {lastUpdatedDate}");
            }
        }

        private void ExecutePartialSync(DateTime lastCreatedDate, DateTime lastUpdatedDate, CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync with lastCreatedDate: {lastCreatedDate} and lastUpdatedDate: {lastUpdatedDate}");

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            var expectedCreatedCount = _apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsCreatedSinceCount(lastCreatedDate, cancellationToken).Result;
            var createdCursor = _apprenticeshipApplicationsRepository.GetAllApprenticeshipApplicationsCreatedSince(lastCreatedDate, cancellationToken).Result;
            ProcessApplications(createdCursor, expectedCreatedCount, lastCreatedDate, lastUpdatedDate, vacancyIds, (applicationTable, applications) => _genericSyncRespository.BulkInsert(applicationTable, applications), cancellationToken);

            var expectedUpdatedCount = _apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsUpdatedSinceCount(lastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _apprenticeshipApplicationsRepository.GetAllApprenticeshipApplicationsUpdatedSince(lastUpdatedDate, cancellationToken).Result;
            ProcessApplications(updatedCursor, expectedUpdatedCount, lastCreatedDate, lastUpdatedDate, vacancyIds, (applicationTable, applications) => _genericSyncRespository.BulkUpdate(applicationTable, applications), cancellationToken);
        }

        private void ProcessApplications(IAsyncCursor<ApprenticeshipApplication> cursor, long expectedCount, DateTime lastCreatedDate, DateTime lastUpdatedDate, HashSet<int> vacancyIds, Action<ITableDetails, IEnumerable<IDictionary<string, object>>> bulkAction, CancellationToken cancellationToken)
        {
            var applicationTable = new ApplicationTable();

            var candidates = new Dictionary<Guid, Candidate>();

            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if(batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} Applications");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                _logService.Info("Loading candidates");
                var candidatesCursor = _candidateRepository.GetCandidatesByIds(batch.Select(a => a.CandidateId).Except(candidates.Keys), cancellationToken).Result;
                while (candidatesCursor.MoveNextAsync(cancellationToken).Result)
                {
                    var candidatesBatch = candidatesCursor.Current.ToList();
                    foreach (var candidate in candidatesBatch)
                    {
                        candidates[candidate.Id] = candidate;
                    }
                    _logService.Info($"Completed loading {candidatesBatch.Count} candidates");
                }

                var applications = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidates.ContainsKey(a.CandidateId)).Select(a => a.ToApplicationDictionary(candidates[a.CandidateId])).ToList();
                count += applications.Count;
                _logService.Info($"Inserting {applications.Count} Applications");
                try
                {
                    bulkAction(applicationTable, applications);

                    _genericSyncRespository.BulkInsert(applicationTable, applications);
                }
                catch (Exception ex)
                {
                    _logService.Error("Error while processing applications", ex);
                }

                lastCreatedDate = maxDateCreated > lastCreatedDate ? maxDateCreated : lastCreatedDate;
                lastUpdatedDate = maxDateUpdated > lastUpdatedDate ? maxDateUpdated : lastUpdatedDate;
                _syncRepository.SetApplicationLastCreatedDate(lastCreatedDate);
                _syncRepository.SetApplicationLastUpdatedDate(lastUpdatedDate);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed {applications.Count} Applications and {count} Applications out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. lastCreatedDate: {lastCreatedDate} lastUpdatedDate: {lastUpdatedDate}");
            }
        }
    }
}