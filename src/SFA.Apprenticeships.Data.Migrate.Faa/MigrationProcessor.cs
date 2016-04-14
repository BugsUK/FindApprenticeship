namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Linq;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
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

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            _logService.Info("Initialisation");

            var persistentConfig = configurationService.Get<MigrateFromAvmsConfiguration>();

            var sourceDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.TargetConnectionString);
            _genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            _vacancyRepository = new VacancyRepository(targetDatabase);
            _candidateRepository = new CandidateRepository(configurationService, _logService);
            _apprenticeshipApplicationsRepository = new ApprenticeshipApplicationsRepository(configurationService);
        }

        public void ExecuteFullSync()
        {
            var applicationTable = new ApplicationTable();
            _genericSyncRespository.DeleteAll(applicationTable);

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            _logService.Info("Loading candidates");
            var candidates = _candidateRepository.GetAllCandidatesAsync().Result;
            _logService.Info($"Completed loading {candidates.Count} candidates");

            var expectedCount = _apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsCount().Result;
            var count = 0;

            var cursor = _apprenticeshipApplicationsRepository.GetAllApprenticeshipApplications().Result;
            while (cursor.MoveNextAsync().Result)
            {
                var batch = cursor.Current.ToList();
                _logService.Info($"Processing {batch.Count} Applications");

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
                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Inserted {applications.Count} Applications and {count} Applications out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete");
            }
        }

        public void ExecutePartialSync(DateTime lastUpdatedDateTime)
        {
            var applicationTable = new ApplicationTable();
            _genericSyncRespository.DeleteAll(applicationTable);

            _logService.Info("Loading Vacancy Ids");
            var vacancyIds = _vacancyRepository.GetAllVacancyIds();
            _logService.Info($"Completed loading {vacancyIds.Count} Vacancy Ids");

            _logService.Info("Loading candidates");
            var candidates = _candidateRepository.GetAllCandidatesAsync().Result;
            _logService.Info($"Completed loading {candidates.Count} candidates");

            var expectedCount = _apprenticeshipApplicationsRepository.GetApprenticeshipApplicationsCount().Result;
            var count = 0;

            var cursor = _apprenticeshipApplicationsRepository.GetAllApprenticeshipApplications().Result;
            while (cursor.MoveNextAsync().Result)
            {
                var batch = cursor.Current.ToList();
                _logService.Info($"Processing {batch.Count} Applications");

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
                var percentage = ((double)count / expectedCount) * 100;
                _logService.Info($"Inserted {applications.Count} Applications and {count} Applications out of {expectedCount} in total. {Math.Round(percentage, 2)}% complete");
            }
        }
    }
}