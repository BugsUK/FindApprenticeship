namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Configuration;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Sql;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class MigrationProcessor
    {
        private readonly ILogService _logService;
        private readonly IList<IMigrationProcessor> _migrationProcessors;
        private readonly bool _isEnabled;

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            _logService.Info("Initialisation");

            var configuration = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();

            if (!(_isEnabled = configuration.IsEnabled))
            {
                return;
            }

            // TODO: AG: refactor set-up out of constructor (should be done first time in Execute() method).

            //Ensure date precision is honoured
            Dapper.SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);

            var sourceDatabase = new GetOpenConnectionFromConnectionString(configuration.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(configuration.TargetConnectionString);

            var syncRepository = new SyncRepository(targetDatabase);
            var genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            var applicationMappers = new ApplicationMappers(_logService);

            _migrationProcessors = new List<IMigrationProcessor>
            {
                new CandidateMigrationProcessor(new CandidateMappers(_logService), syncRepository, genericSyncRespository, targetDatabase, configurationService, logService),
                new VacancyApplicationsMigrationProcessor(new TraineeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, configurationService, logService),
                new VacancyApplicationsMigrationProcessor(new ApprenticeshipApplicationsUpdater(syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, configurationService, logService)
            };
        }

        public void Execute(CancellationToken cancellationToken)
        {
            if (!_isEnabled)
            {
                _logService.Warn("Migrate.Faa process is disabled.");
                cancellationToken.WaitHandle.WaitOne();
                return;
            }

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

                // TODO: AG: configure.
                Thread.Sleep(10000);
            }

            _logService.Info("DoAll Cancelled");
        }
    }
}
