namespace SFA.Apprenticeships.Infrastructure.Logging.Targets
{
    using System.Text;
    using Microsoft.ServiceBus.Messaging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using StructureMap;
    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using IoC;

    [Target("AzureEventHubTarget")]
    public class AzureEventHubTarget : TargetWithLayout
    {
        private EventHubClient _eventHubClient;
        private IConfigurationManager _configManager;

        private object _lock = new object();

        [RequiredParameter]
        public string EventHubConnectionStringSettingName { get; set; }

        [RequiredParameter]
        public string EventHubPath { get; set; }

        public AzureEventHubTarget()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            _configManager = container.GetInstance<IConfigurationManager>();
        }

        protected override void Write(LogEventInfo logEvent)
        {
#pragma warning disable 4014
            SendAsync(logEvent);
#pragma warning restore 4014
        }

        private async void SendAsync(LogEventInfo logEvent)
        {
            var eventHubClient = GetEventHubClient();
            var json = Layout.Render(logEvent);

            using (var eventHubData = new EventData(Encoding.UTF8.GetBytes(json)))
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
                        var connectionString = _configManager.GetAppSetting(EventHubConnectionStringSettingName);

                        _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, EventHubPath);
                    }
                }
            }

            return _eventHubClient;
        }
    }
}