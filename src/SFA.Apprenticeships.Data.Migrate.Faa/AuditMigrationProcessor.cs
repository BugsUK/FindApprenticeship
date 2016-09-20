namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System.Threading;
    using Application.Interfaces;
    using Entities.Sql;
    using Infrastructure.Repositories.Sql.Common;
    using Repository.Mongo;
    using Repository.Sql;

    public class AuditMigrationProcessor : IMigrationProcessor
    {
        private readonly ILogService _logService;

        private readonly SyncRepository _syncRepository;
        private readonly AuditRepository _auditRepository;

        public AuditMigrationProcessor(SyncRepository syncRepository, IGetOpenConnection targetDatabase, IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            _syncRepository = syncRepository;
            _auditRepository = new AuditRepository(configurationService, logService);
        }

        public void Process(CancellationToken cancellationToken)
        {
            var syncParams = _syncRepository.GetSyncParams();
            if (syncParams.IsValidForAuditIncrementalSync)
            {
                ExecuteIncrementalSync(syncParams, cancellationToken);
            }
            else
            {
                ExecuteFullSync(syncParams, cancellationToken);
            }
        }

        private void ExecuteFullSync(SyncParams syncParams, CancellationToken cancellationToken)
        {
            _logService.Warn($"ExecuteFullSync on audits collection with LastAuditEventDate: {syncParams.LastAuditEventDate}");

            var expectedCount = _auditRepository.GetAuditItemsCount(cancellationToken).Result;
        }

        private void ExecuteIncrementalSync(SyncParams syncParams, CancellationToken cancellationToken)
        {
            _logService.Info($"ExecutePartialSync on candidates collection with LastAuditEventDate: {syncParams.LastAuditEventDate}");

            
        }
    }
}