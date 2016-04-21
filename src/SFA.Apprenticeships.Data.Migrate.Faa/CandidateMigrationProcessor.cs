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
        private readonly SyncRepository _syncRepository;

        private readonly ITableSpec _candidateTable = new CandidateTable();

        public CandidateMigrationProcessor(ICandidateMappers candidateMappers, SyncRepository syncRepository, IGenericSyncRespository genericSyncRespository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _candidateMappers = candidateMappers;
            _syncRepository = syncRepository;
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

        private void BulkInsert(IList<CandidatePersonDictionary> candidates)
        {
            _genericSyncRespository.BulkInsert(_candidateTable, candidates.Select(c => c.Candidate));
        }

        private void BulkDelete(IList<CandidatePersonDictionary> candidates)
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

            using (var connection = (SqlConnection)_targetDatabase.GetOpenConnection())
            {
                //Delete application history associated with application
                connection.Execute($@"DELETE FROM {_applicationHistoryTable.Name} WHERE ApplicationId IN @applicationIds", new { applicationIds });

                //Delete applications
                connection.Execute($@"DELETE FROM {_applicationTable.Name} WHERE ApplicationId IN @applicationIds", new { applicationIds });
            }*/
        }

        private void ProcessCandidates(IList<CandidateUser> candidateUsers, long expectedCount, Action<IList<CandidatePersonDictionary>> bulkAction, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var count = 0;

            var maxDateCreated = candidateUsers.Max(c => c.Candidate.DateCreated);
            var maxDateUpdated = candidateUsers.Max(c => c.Candidate.DateUpdated) ?? DateTime.MinValue;

            var applicationsWithHistory = candidateUsers.Where(c => c.User.Status >= 20).Select(c => _candidateMappers.MapCandidatePersonDictionary(c)).ToList();
            count += applicationsWithHistory.Count;
            _logService.Info($"Processing {applicationsWithHistory.Count} candidates");
            try
            {
                bulkAction(applicationsWithHistory);
            }
            catch (Exception ex)
            {
                _logService.Error("Error while processing candidates. Trying a delete then retry", ex);
                //Propagate exception back up the stack rather than silently continuing
                BulkDelete(applicationsWithHistory);
                bulkAction(applicationsWithHistory);
            }

            var syncParams = _syncRepository.GetSyncParams();
            syncParams.CandidateLastCreatedDate = maxDateCreated > syncParams.CandidateLastCreatedDate ? maxDateCreated : syncParams.CandidateLastCreatedDate;
            syncParams.CandidateLastUpdatedDate = maxDateUpdated > syncParams.CandidateLastUpdatedDate ? maxDateUpdated : syncParams.CandidateLastUpdatedDate;
            _syncRepository.SetCandidateSyncParams(syncParams);

            var percentage = ((double)count / expectedCount) * 100;
            _logService.Info($"Processed batch of {applicationsWithHistory.Count} candidates and {count} candidates out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete. LastCreatedDate: {syncParams.CandidateLastCreatedDate} LastUpdatedDate: {syncParams.CandidateLastUpdatedDate}");
        }
    }
}