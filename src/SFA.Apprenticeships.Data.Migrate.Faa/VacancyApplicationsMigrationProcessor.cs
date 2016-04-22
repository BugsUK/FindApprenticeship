namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading;
    using Dapper;
    using Entities;
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
        private readonly IApplicationMappers _applicationMappers;
        private readonly IGenericSyncRespository _genericSyncRespository;
        private readonly IGetOpenConnection _targetDatabase;
        private readonly ILogService _logService;
        
        private readonly VacancyRepository _vacancyRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly ApplicationRepository _applicationRepository;
        private readonly ApplicationHistoryRepository _applicationHistoryRepository;
        private readonly VacancyApplicationsRepository _vacancyApplicationsRepository;

        private readonly ITableSpec _applicationTable = new ApplicationTable();
        private readonly ITableSpec _applicationHistoryTable = new ApplicationHistoryTable();

        public VacancyApplicationsMigrationProcessor(IVacancyApplicationsUpdater vacancyApplicationsUpdater, IApplicationMappers applicationMappers, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _vacancyApplicationsUpdater = vacancyApplicationsUpdater;
            _applicationMappers = applicationMappers;
            _genericSyncRespository = genericSyncRespository;
            _targetDatabase = targetDatabase;
            _logService = logService;

            _vacancyRepository = new VacancyRepository(targetDatabase);
            _candidateRepository = new CandidateRepository(targetDatabase);
            _applicationRepository = new ApplicationRepository(targetDatabase);
            _applicationHistoryRepository = new ApplicationHistoryRepository(targetDatabase);
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

            //TODO: These deletes would have to be done outside of this class as it affects traineeships and apprenticeships at the same time
            //_genericSyncRespository.DeleteAll(_applicationHistoryTable);
            //_genericSyncRespository.DeleteAll(_applicationTable);

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            _logService.Info("Loading candidates");
            var candidateIds = _candidateRepository.GetAllCandidateIds();
            _logService.Info($"Completed loading {candidateIds.Count} candidates");

            var expectedCount = _vacancyApplicationsRepository.GetVacancyApplicationsCount(cancellationToken).Result;
            var cursor = _vacancyApplicationsRepository.GetAllVacancyApplications(cancellationToken).Result;
            ProcessApplications(cursor, expectedCount, vacancyIds, candidateIds, BulkInsert, cancellationToken);
        }

        private void ExecuteIncrementalSync(CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on {_vacancyApplicationsUpdater.CollectionName} collection with LastCreatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastCreatedDate} LastUpdatedDate: {_vacancyApplicationsUpdater.VacancyApplicationLastUpdatedDate}");

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            var candidates = new Dictionary<Guid, int>();

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

        private void BulkInsert(IList<ApplicationWithHistory> applicationsWithHistory)
        {
            _genericSyncRespository.BulkInsert(_applicationTable, applicationsWithHistory.Select(a => _applicationMappers.MapApplicationDictionary(a.Application)));

            _genericSyncRespository.BulkInsert(_applicationHistoryTable, applicationsWithHistory.SelectMany(a => a.ApplicationHistory.MapApplicationHistoryDictionary()));
        }

        private void BulkUpdate(IList<ApplicationWithHistory> applicationsWithHistory)
        {
            //Updates can edit the ApplicationId value so delete the existing records and recreate
            BulkDelete(applicationsWithHistory);

            //Then create the new ones
            BulkInsert(applicationsWithHistory);
        }

        private void BulkDelete(IList<ApplicationWithHistory> applicationsWithHistory)
        {
            /*var applicationIds = _targetDatabase.Query<int>($@"SELECT ApplicationId FROM {_applicationTable.Name} WHERE ApplicationGuid IN @applicationGuids", new { applicationGuids = applicationsWithHistory.Select(a => (Guid)a.Application["ApplicationGuid"]) }).ToList();
            //Haven't identified why but there are cases where we end up with duplicate applications that aren't identified correctly by their guid so select those Ids too
            foreach (var application in applicationsWithHistory.Select(a => a.Application))
            {
                var applicationId = _targetDatabase.Query<int?>($@"SELECT ApplicationId FROM {_applicationTable.Name} WHERE VacancyId = @vacancyId AND CandidateId = @candidateId", new { vacancyId = application["VacancyId"], candidateId = application["CandidateId"] }).SingleOrDefault();
                if (applicationId.HasValue && !applicationIds.Contains(applicationId.Value))
                {
                    applicationIds.Add(applicationId.Value);
                }
            }

            using (var connection = (SqlConnection) _targetDatabase.GetOpenConnection())
            {
                //Delete application history associated with application
                connection.Execute($@"DELETE FROM {_applicationHistoryTable.Name} WHERE ApplicationId IN @applicationIds", new { applicationIds });

                //Delete applications
                connection.Execute($@"DELETE FROM {_applicationTable.Name} WHERE ApplicationId IN @applicationIds", new { applicationIds });
            }*/
        }

        private void ProcessApplications(IAsyncCursor<VacancyApplication> cursor, long expectedCount, HashSet<int> vacancyIds, IDictionary<Guid, int> candidateIds, Action<IList<ApplicationWithHistory>> bulkAction, CancellationToken cancellationToken)
        {
            var count = 0;
            while (cursor.MoveNextAsync(cancellationToken).Result && !cancellationToken.IsCancellationRequested)
            {
                var batch = cursor.Current.ToList();
                if (batch.Count == 0) continue;
                _logService.Info($"Processing {batch.Count} {_vacancyApplicationsUpdater.CollectionName}");

                var maxDateCreated = batch.Max(a => a.DateCreated);
                var maxDateUpdated = batch.Max(a => a.DateUpdated) ?? DateTime.MinValue;

                LoadCandidates(candidateIds, batch);

                var applicationIds = _applicationRepository.GetApplicationIdsByGuid(batch.Select(a => a.Id));
                var applicationHistoryIds = _applicationHistoryRepository.GetApplicationHistoryIdsByApplicationIds(applicationIds.Values);
                var applicationsWithHistory = batch.Where(a => vacancyIds.Contains(a.Vacancy.Id) && candidateIds.ContainsKey(a.CandidateId)).Select(a => _applicationMappers.MapApplicationWithHistory(a, candidateIds[a.CandidateId], applicationIds, applicationHistoryIds)).ToList();

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
            }
        }

        private void LoadCandidates(IDictionary<Guid, int> candidateIds, IEnumerable<VacancyApplication> vacancyApplications)
        {
            var newCandidateIds = vacancyApplications.Select(a => a.CandidateId).Except(candidateIds.Keys).ToArray();
            if (newCandidateIds.Any())
            {
                _logService.Info($"Loading {newCandidateIds.Length} candidates");
                foreach (var candidateIdKvp in _candidateRepository.GetCandidateIdsByGuid(newCandidateIds))
                {
                    candidateIds.Add(candidateIdKvp);
                }
                _logService.Info($"Completed loading batch of {newCandidateIds.Length} candidates");
            }
        }
    }
}