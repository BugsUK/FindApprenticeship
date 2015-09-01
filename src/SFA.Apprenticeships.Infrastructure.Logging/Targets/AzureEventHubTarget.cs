namespace SFA.Apprenticeships.Infrastructure.Logging.Targets
{
    using System.Text;
    using Microsoft.Azure;
    using Microsoft.ServiceBus.Messaging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    [Target("AzureEventHubTarget")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        object _lock = new object();
        EventHubClient _eventHubClient;

        [RequiredParameter]
        public string EventHubConnectionStringSettingName { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        public string PartitionKey { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
#pragma warning disable 4014
            SendAsync(PartitionKey, logEvent);
#pragma warning restore 4014
        }

        private async void SendAsync(string partitionKey, LogEventInfo logEvent)
        {
            var eventHubClient = GetEventHubClient();
            var logMessage = Layout.Render(logEvent);

            using (var eventHubData = new EventData(Encoding.UTF8.GetBytes(logMessage))
            {
                PartitionKey = partitionKey
            })
            {
                foreach (var key in logEvent.Properties.Keys)
                {
                    eventHubData.Properties.Add(key.ToString(), logEvent.Properties[key]);
                }

                await eventHubClient.SendAsync(eventHubData);
            }
        }

        private EventHubClient GetEventHubClient()
        {
            if (_eventHubClient == null)
            {
                lock (_lock)
                {
                    if (_eventHubClient == null)
                    {
                        var connectionString = CloudConfigurationManager.GetSetting(EventHubConnectionStringSettingName);
                        var messsagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);

                        _eventHubClient = messsagingFactory.CreateEventHubClient(EventHubPath);
                    }
                }
            }

            return _eventHubClient;
        }
    }
}
