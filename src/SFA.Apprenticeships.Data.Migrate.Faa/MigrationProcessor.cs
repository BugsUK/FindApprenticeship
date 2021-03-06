﻿namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Threading;
    using Configuration;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Sql;
    using Application.Interfaces;

    public class MigrationProcessor
    {
        private readonly ILogService _logService;

        private readonly SyncRepository _syncRepository;
        private readonly CandidateMigrationProcessor _candidateMigrationProcessor;
        private readonly VacancyApplicationsMigrationProcessor _traineeshipApplicationsMigrationProcessor;
        private readonly VacancyApplicationsMigrationProcessor _apprenticeshipApplicationsMigrationProcessor;
        private readonly AuditMigrationProcessor _auditMigrationProcessor;

        public MigrationProcessor(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            var configuration = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();

            //Ensure date precision is honoured
            Dapper.SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);

            var sourceDatabase = new GetOpenConnectionFromConnectionString(configuration.SourceConnectionString);
            var targetDatabase = new GetOpenConnectionFromConnectionString(configuration.TargetConnectionString);
            var genericSyncRespository = new GenericSyncRespository(_logService, sourceDatabase, targetDatabase);

            _syncRepository = new SyncRepository(targetDatabase);
            
            var applicationMappers = new ApplicationMappers(_logService);

            _candidateMigrationProcessor = new CandidateMigrationProcessor(new CandidateMappers(_logService), _syncRepository, genericSyncRespository, targetDatabase, configurationService, _logService);
            _traineeshipApplicationsMigrationProcessor = new VacancyApplicationsMigrationProcessor(new TraineeshipApplicationsUpdater(_syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, configurationService, _logService);
            _apprenticeshipApplicationsMigrationProcessor = new VacancyApplicationsMigrationProcessor(new ApprenticeshipApplicationsUpdater(_syncRepository), applicationMappers, genericSyncRespository, sourceDatabase, targetDatabase, configurationService, _logService);
            _auditMigrationProcessor = new AuditMigrationProcessor(_syncRepository, targetDatabase, configurationService, _logService);

            _logService.Info("Initialisation");
        }

        public void ExecuteCandidateMigrationProcessor(CancellationToken cancellationToken)
        {
            ExecuteProcessor(_candidateMigrationProcessor, cancellationToken);
        }

        public void ExecuteTraineeshipApplicationsMigrationProcessor(CancellationToken cancellationToken)
        {
            ExecuteProcessor(_traineeshipApplicationsMigrationProcessor, cancellationToken);
        }

        public void ExecuteApprenticeshipApplicationsMigrationProcessor(CancellationToken cancellationToken)
        {
            ExecuteProcessor(_apprenticeshipApplicationsMigrationProcessor, cancellationToken);
        }

        public void ExecuteAuditMigrationProcessor(CancellationToken cancellationToken)
        {
            ExecuteProcessor(_auditMigrationProcessor, cancellationToken);
        }

        private void ExecuteProcessor(IMigrationProcessor migrationProcessor, CancellationToken cancellationToken)
        {
            _logService.Info($"Executing {migrationProcessor}");

            try
            {
                var lastSyncVersion = _syncRepository.GetSyncParams().LastSyncVersion;
                if (lastSyncVersion.HasValue && lastSyncVersion > 0)
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

            _logService.Info($"{migrationProcessor} executed");
        }
    }
}
