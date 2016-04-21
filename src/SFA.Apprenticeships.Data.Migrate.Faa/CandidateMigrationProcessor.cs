namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Entities;
    using Entities.Mongo;
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

        private readonly ITableSpec _candidateTable = new CandidateTable();

        public CandidateMigrationProcessor(ICandidateMappers candidateMappers, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _candidateMappers = candidateMappers;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _configurationService = configurationService;
            _logService = logService;

            _candidateRepository = new CandidateRepository(configurationService, _logService);
        }

        public void Process(CancellationToken cancellationToken)
        {
            ExecuteFullSync(cancellationToken);
        }

        private void ExecuteFullSync(CancellationToken cancellationToken)
        {
            //_logService.Warn($"ExecuteFullSync on candidates collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            //TODO: This delete would have to be done outside of this class as it affects traineeships and apprenticeships at the same time
            //_genericSyncRespository.DeleteAll(_candidateTable);

            var expectedCount = _candidateRepository.GetCandidatesCount(cancellationToken).Result;
            var candidateUsers = _candidateRepository.GetAllCandidateUsers(cancellationToken).Result;
            ProcessCandidates(candidateUsers, expectedCount, BulkInsert, cancellationToken);
        }

        private void BulkInsert(IList<IDictionary<string, object>> candidates)
        {
            _genericSyncRespository.BulkInsert(_candidateTable, candidates);
        }

        private void ProcessCandidates(IList<CandidateUser> candidateUsers, long expectedCount, Action<IList<IDictionary<string, object>>> bulkAction, CancellationToken cancellationToken)
        {
            /*var count = 0;
            while (candidateUsers.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = candidateUsers.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} {_vacancyApplicationsUpdater.CollectionName}");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                LoadCandidates(candidates, batch, cancellationToken);

                var applicationsWithHistory = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidates.ContainsKey(a.CandidateId)).Select(a => _applicationMappers.MapApplicationWithHistoryDictionary(a, candidates[a.CandidateId])).ToList();
                count += applicationsWithHistory.Count;
                _logService.Info($"Processing {applicationsWithHistory.Count} {_vacancyApplicationsUpdater.CollectionName}");
                try
                {
                    bulkAction(applicationsWithHistory);
                }
                catch (Exception ex)
                {
                    _logService.Error($"Error while processing {_vacancyApplicationsUpdater.CollectionName}. Trying a delete then retry", ex);
                    //Propagate exception back up the stack rather than silently continuing
                    BulkDelete(applicationsWithHistory);
                    bulkAction(applicationsWithHistory);
                }

                _vacancyApplicationsUpdater.UpdateSyncDates(maxDateCreated, maxDateUpdated);

                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Processed batch of {applicationsWithHistory.Count} {_vacancyApplicationsUpdater.CollectionName} and {count} {_vacancyApplicationsUpdater.CollectionName} out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");
            }*/
        }
    }
}