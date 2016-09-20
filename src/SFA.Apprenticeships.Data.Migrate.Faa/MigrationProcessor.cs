namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Configuration;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Sql;
    using Application.Interfaces;

    public class MigrationProcessor
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogService _logService;

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _configurationService = configurationService;
            _logService = logService;

            _logService.Info("Initialisation");
        }

        public void Execute(CancellationToken cancellationToken)
        {
            var configuration = _configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();
            
            if (!configuration.IsEnabled)
            {
                _logService.Warn("Migrate.Faa process is disabled.");
                cancellationToken.WaitHandle.WaitOne();
                return;
            }

            //Ensure date precision is honoured
            Dapper.SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);

            var sourceDatabase = new GetOpenConnectionFromConnectionString(configuration.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(configuration.TargetConnectionString);

            var syncRepository = new SyncRepository(targetDatabase);
            var genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            var applicationMappers = new ApplicationMappers(_logService);

            var migrationProcessors = new List<IMigrationProcessor>
            {
                //new CandidateMigrationProcessor(new CandidateMappers(_logService), syncRepository, genericSyncRespository, targetDatabase, _configurationService, _logService),
                //new VacancyApplicationsMigrationProcessor(new TraineeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, _configurationService, _logService),
                //new VacancyApplicationsMigrationProcessor(new ApprenticeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, _configurationService, _logService),
                new AuditMigrationProcessor(syncRepository, targetDatabase, _configurationService, _logService)
            };

            _logService.Info("Execute Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var lastSyncVersion = syncRepository.GetSyncParams().LastSyncVersion;
                    if (lastSyncVersion.HasValue && lastSyncVersion > 0)
                    {
                        foreach (var migrationProcessor in migrationProcessors)
                        {
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                migrationProcessor.Process(cancellationToken);
                            }
                        }
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

                Thread.Sleep(configuration.SleepTimeBetweenCyclesInSeconds * 1000);
            }

            _logService.Info("DoAll Cancelled");
        }
    }
}
