namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using Common.Configuration;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Processors;
    using Services;
    using StructureMap;
    using Common.IoC;
    using Logging.IoC;
    using SFA.Infrastructure.Interfaces;
    using Logging;

    public class WorkerRole : RoleEntryPoint
    {
        private const string ProcessName = "Log Processor";

        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();

        private Container _container;
        private ILogService _logService;
        private IConfigurationService _configurationService;
        private EventProcessorHost _eventProcessorHost;

        public override bool OnStart()
        {
            Initialise();

            return base.OnStart();
        }

        public override void Run()
        {
            StartLogEventHubProcessor();
            _cancelSource.Token.WaitHandle.WaitOne();
        }

        public override void OnStop()
        {
            StopLogEventHubProcessor();
            _cancelSource.Cancel();

            base.OnStop();
        }

        private void Initialise()
        {
            try
            {
                ServicePointManager.DefaultConnectionLimit = 12;
                VersionLogging.SetVersion();

                InitializeIoC();
            }
            catch (Exception e)
            {
                if (_logService == null)
                {
                    Trace.TraceError("{0} failed to initialise: \"{1}\"", ProcessName, e);
                }
                else
                {
                    _logService.Error(ProcessName + " failed to initialise", e);
                }

                throw;
            }
        }

        private void InitializeIoC()
        {
            var tempContainer = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationManager = tempContainer.GetInstance<IConfigurationManager>();
            var configurationStorageConnectionString =
                configurationManager.GetAppSetting<string>("ConfigurationStorageConnectionString");

            _container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
                x.AddRegistry<LoggingRegistry>();
            });

            _configurationService = _container.GetInstance<IConfigurationService>();
            _logService = _container.GetInstance<ILogService>();
        }

        private void StartLogEventHubProcessor()
        {
            _logService.Info("Starting log event hub processor");

            var configuration = _configurationService.Get<Configuration.AzureEventHubLogIndexerConfiguration>();

            _eventProcessorHost = new EventProcessorHost(
                Environment.MachineName,
                configuration.EventHubPath,
                configuration.ConsumerGroupName,
                configuration.EventHubConnectionString,
                configuration.StorageConnectionString);

            var indexerService = new LogEventIndexerService(_configurationService);
            var eventProcessorFactory = new LogEventProcessorFactory(_logService, indexerService);

            _eventProcessorHost.RegisterEventProcessorFactoryAsync(eventProcessorFactory);

            _logService.Info("Started log event hub processor");
        }

        private void StopLogEventHubProcessor()
        {
            _logService.Debug("Stopping log event hub processor");

            _eventProcessorHost.UnregisterEventProcessorAsync().Wait();

            _logService.Debug("Stopped log event hub processor");
        }
    }
}