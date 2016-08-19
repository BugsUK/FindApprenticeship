namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using Azure.Common.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    using Tasks;

    public class MonitorControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IMonitorTasksRunner _monitorTasksRunner;
        private readonly IConfigurationService _configurationService;

        public MonitorControlQueueConsumer(IJobControlQueue<StorageQueueMessage> messageService,
            IMonitorTasksRunner monitorTasksRunner, IConfigurationService configurationService, ILogService logger)
            : base(messageService, logger, "Monitor", ScheduledJobQueues.Monitor)
        {
            _monitorTasksRunner = monitorTasksRunner;
            _configurationService = configurationService;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var monitorScheduleMessage = GetLatestQueueMessage();

                if (monitorScheduleMessage != null)
                {
                    var monitorConfig = _configurationService.Get<MonitorConfiguration>();
                    if (monitorConfig != null && monitorConfig.IsEnabled)
                    {
                        _monitorTasksRunner.RunMonitorTasks();
                    }

                    MessageService.DeleteMessage(ScheduledJobQueues.Monitor, monitorScheduleMessage.MessageId, monitorScheduleMessage.PopReceipt);
                }
            });
        }
    }
}