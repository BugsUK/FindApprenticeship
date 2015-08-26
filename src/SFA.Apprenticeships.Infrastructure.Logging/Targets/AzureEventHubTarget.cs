namespace SFA.Apprenticeships.Infrastructure.Logging.Targets
{
    using System.Text;
    using Microsoft.Azure;
    using Microsoft.ServiceBus.Messaging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    [Target("AzureEventHub")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        EventHubClient _eventHubClient = null;
        MessagingFactory _messsagingFactory = null;

        [RequiredParameter]
        public string EventHubConnectionStringSettingName { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        /// <summary>
        /// PartitionKey is optional. If no partition key is supplied the log messages are sent to eventhub
        /// and distributed to various partitions in a round robin manner.
        /// </summary>
        public string PartitionKey { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
#pragma warning disable 4014
            SendAsync(PartitionKey, logEvent);
#pragma warning restore 4014
        }

        private async void SendAsync(string partitionKey, LogEventInfo logEvent)
        {
            if (this._messsagingFactory == null)
            {
                var connectionString = CloudConfigurationManager.GetSetting(EventHubConnectionStringSettingName);
                this._messsagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
            }

            if (this._eventHubClient == null)
            {
                this._eventHubClient = this._messsagingFactory.CreateEventHubClient(EventHubPath);
            }

            string logMessage = this.Layout.Render(logEvent);

            using (var eventHubData = new EventData(Encoding.UTF8.GetBytes(logMessage))
            {
                PartitionKey = partitionKey
            })
            {
                foreach (var key in logEvent.Properties.Keys)
                {
                    eventHubData.Properties.Add(key.ToString(), logEvent.Properties[key]);
                }

                await _eventHubClient.SendAsync(eventHubData);
            }
        }
    }
}
