namespace SFA.Apprenticeships.Infrastructure.Log
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using Application.Interfaces.Logging;
    using Apprenticeships.Domain.Interfaces.Configuration;
    using Common.IoC;
    using Elastic.Common.Configuration;
    using Elastic.Common.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Processors;
    using Services;
    using StructureMap;

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
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });

            _configurationService = _container.GetInstance<IConfigurationService>();
            _logService = _container.GetInstance<ILogService>();
        }

        private void StartLogEventHubProcessor()
        {
            _logService.Info("Starting log event hub processor");

            var configuration = _configurationService.Get<Configuration.AzureEventHubLogProcessorConfiguration>();

            _eventProcessorHost = new EventProcessorHost(
                Environment.MachineName,
                configuration.EventHubPath,
                configuration.ConsumerGroupName,
                configuration.EventHubConnectionString,
                configuration.StorageConnectionString);

            var elasticSearchClientFactory = _container.GetInstance<IElasticsearchClientFactory>();
            var indexerService = new LogEventIndexerService(elasticSearchClientFactory);
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