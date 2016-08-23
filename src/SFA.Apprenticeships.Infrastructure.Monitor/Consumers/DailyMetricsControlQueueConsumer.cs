namespace SFA.Apprenticeships.Infrastructure.Monitor.Consumers
{
    using System.Threading.Tasks;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Domain.Interfaces.Messaging;
    using Azure.Common.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    using Tasks;

    public class DailyMetricsControlQueueConsumer : AzureControlQueueConsumer
    {
        private readonly IDailyMetricsTasksRunner _dailyMetricsTasksRunner;
        private readonly IConfigurationService _configurationService;

        public DailyMetricsControlQueueConsumer(
            IJobControlQueue<StorageQueueMessage> messageService,
            IDailyMetricsTasksRunner dailyMetricsTasksRunner,
            IConfigurationService configurationService,
            ILogService logger)
            : base(messageService, logger, "DailyMetrics", ScheduledJobQueues.DailyMetrics)
        {
            _dailyMetricsTasksRunner = dailyMetricsTasksRunner;
            _configurationService = configurationService;
        }

        public Task CheckScheduleQueue()
        {
            return Task.Run(() =>
            {
                var monitorScheduleMessage = GetLatestQueueMessage();

                if (monitorScheduleMessage != null)
                {
                    //Delete message as metrics queries can take a long time and the message can be read several times causing multiple code executions
                    MessageService.DeleteMessage(ScheduledJobQueues.DailyMetrics, monitorScheduleMessage.MessageId, monitorScheduleMessage.PopReceipt);

                    var monitorConfig = _configurationService.Get<MonitorConfiguration>();
                    if (monitorConfig != null && monitorConfig.IsDailyMetricsEnabled)
                    {
                        _dailyMetricsTasksRunner.RunDailyMetricsTasks();
                    }
                }
            });
        }
    }
}