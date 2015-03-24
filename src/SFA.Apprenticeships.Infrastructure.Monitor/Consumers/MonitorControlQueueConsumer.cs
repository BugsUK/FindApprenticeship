namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Azure.Common.Messaging;
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
                    if (IsMonitorEnabled())
                    {
                        _monitorTasksRunner.RunMonitorTasks();
                    }

                    MessageService.DeleteMessage(ScheduledJobQueues.Monitor, monitorScheduleMessage.MessageId, monitorScheduleMessage.PopReceipt);
                }
            });
        }

        private bool IsMonitorEnabled()
        {   
            return _configurationService.GetCloudAppSetting<bool>("MonitorEnabled");
        }
    }
}