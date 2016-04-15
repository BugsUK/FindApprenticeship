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

    public class VacancyApplicationsMigrationProcessor : IMigrationProcessor
    {
        private readonly IVacancyApplicationsUpdater _vacancyApplicationsUpdater;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly ILogService _logService;
        
        private readonly VacancyRepository _vacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly VacancyApplicationsRepository _vacancyApplicationsRepository;

        private readonly ITableSpec _applicationTable = new ApplicationTable();

        public VacancyApplicationsMigrationProcessor(IVacancyApplicationsUpdater vacancyApplicationsUpdater, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _vacancyApplicationsUpdater = vacancyApplicationsUpdater;
            _genericSyncRespository = genericSyncRespository;
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
            ProcessApplications(cursor, expectedCount, vacancyIds, candidates, (applicationTable, applications) => _genericSyncRespository.BulkInsert(applicationTable, applications), cancellationToken);
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
            ProcessApplications(createdCursor, expectedCreatedCount, vacancyIds, candidates, (applicationTable, applications) => _genericSyncRespository.BulkInsert(applicationTable, applications), cancellationToken);
            _logService.Info($"Completed processing new {_vacancyApplicationsUpdater.CollectionName}");

            //Updates
            _logService.Info($"Processing updated {_vacancyApplicationsUpdater.CollectionName}");
            var expectedUpdatedCount = _vacancyApplicationsRepository.GetVacancyApplicationsUpdatedSinceCount(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            var updatedCursor = _vacancyApplicationsRepository.GetAllVacancyApplicationsUpdatedSince(_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate, cancellationToken).Result;
            ProcessApplications(updatedCursor, expectedUpdatedCount, vacancyIds, candidates, (applicationTable, applications) => _genericSyncRespository.BulkUpdate(applicationTable, applications), cancellationToken);
            _logService.Info($"Completed processing updated {_vacancyApplicationsUpdater.CollectionName}");
        }

        private void ProcessApplications(IAsyncCursor<VacancyApplication> cursor, long expectedCount, HashSet<int> vacancyIds, IDictionary<Guid, Candidate> candidates, Action<ITableDetails, IEnumerable<IDictionary<string, object>>> bulkAction, CancellationToken cancellationToken)
        {
            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} {_vacancyApplicationsUpdater.CollectionName}");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                var candidateIds = batch.Select(a => a.CandidateId).Except(candidates.Keys).ToArray();
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

                var applications = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidates.ContainsKey(a.CandidateId)).Select(a => a.ToApplicationDictionary(candidates[a.CandidateId])).ToList();
                count += applications.Count;
                _logService.Info($"Processing {applications.Count} {_vacancyApplicationsUpdater.CollectionName}");
                try
                {
                    //TODO: Check if the application now has an Id?
                    bulkAction(_applicationTable, applications);
                }
                catch (Exception ex)
                {
                    _logService.Error($"Error while processing {_vacancyApplicationsUpdater.CollectionName}", ex);
                }

                _vacancyApplicationsUpdater.UpdateSyncDates(maxDateCreated, maxDateUpdated);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {applications.Count} {_vacancyApplicationsUpdater.CollectionName} and {count} {_vacancyApplicationsUpdater.CollectionName} out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");
            }
        }
    }
}