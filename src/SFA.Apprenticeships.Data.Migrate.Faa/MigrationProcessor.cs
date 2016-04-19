namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Sql;
    using SFA.Infrastructure.Interfaces;

    public class MigrationProcessor
    {
        private readonly ILogService _logService;
        private readonly IList<IMigrationProcessor> _migrationProcessors;

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            _logService.Info("Initialisation");

            var persistentConfig = configurationService.Get<MigrateFromAvmsConfiguration>();

            //Ensure date precision is honoured
            Dapper.SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);

            var sourceDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(persistentConfig.TargetConnectionString);

            var syncRepository = new SyncRepository(targetDatabase);
            var genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            var applicationMappers = new ApplicationMappers(targetDatabase);

            _migrationProcessors = new List<IMigrationProcessor>
            {
                new VacancyApplicationsMigrationProcessor(new TraineeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, targetDatabase, configurationService, logService),
                new VacancyApplicationsMigrationProcessor(new ApprenticeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, targetDatabase, configurationService, logService)
            };
        }

        public void Execute(CancellationToken cancellationToken)
        {
            _logService.Info("Execute Started");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var migrationProcessor in _migrationProcessors)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            migrationProcessor.Process(cancellationToken);
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

                Thread.Sleep(10000);
            }

            _logService.Info("DoAll Cancelled");
        }
    }
}