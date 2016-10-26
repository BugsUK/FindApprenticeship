namespace SFA.Apprenticeships.Infrastructure.Migrate.Faa.Consumers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Azure.Common.Messaging;
    using Data.Migrate.Faa;
    using Domain.Interfaces.Messaging;

    public class FaaMigrationControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly ILogService _logService;

        private readonly MigrationProcessor _migrationProcessor;

        public FaaMigrationControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService, IConfigurationService configurationService, ILogService logService) : base(messageService, logService, "FAA Migration", ScheduledJobQueues.FaaMigration)
        {
            _logService = logService;

            _migrationProcessor = new MigrationProcessor(configurationService, _logService);
        }

        public Task CheckScheduleQueue(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var latestScheduledMessage = GetLatestQueueMessage();

                if (latestScheduledMessage == null)
                {
                    _logService.Debug("No scheduled message found on control queue");
                    return;
                }

                _logService.Info("Calling migration processor to migrate data from FAA");

                _migrationProcessor.ExecuteAuditMigrationProcessor(new CancellationToken());
                _migrationProcessor.ExecuteCandidateMigrationProcessor(new CancellationToken());
                _migrationProcessor.ExecuteTraineeshipApplicationsMigrationProcessor(new CancellationToken());
                _migrationProcessor.ExecuteApprenticeshipApplicationsMigrationProcessor(new CancellationToken());

                _logService.Info("Migration processor executed. Check logs for FAA migration completion");
            }, cancellationToken);
        }
    }
}
