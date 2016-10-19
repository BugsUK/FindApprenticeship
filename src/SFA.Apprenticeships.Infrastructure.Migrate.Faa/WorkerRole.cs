namespace SFA.Apprenticeships.Infrastructure.Migrate.Faa
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Application.Interfaces;
    using Azure.ServiceBus;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using StructureMap;
    using Common.IoC;
    using Data.Migrate.Faa;
    using Data.Migrate.Faa.IoC;
    using Logging.IoC;
    using Logging;

    public class WorkerRole : RoleEntryPoint
    {
        private const string ProcessName = "Migrate.Faa";

        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();

        private Container _container;
        private ILogService _logService;

        public override bool OnStart()
        {
            Initialise();

            return base.OnStart();
        }

        public override void Run()
        {
            var configService = _container.GetInstance<IConfigurationService>();
            var processor = new MigrationProcessor(configService, _logService);

            try
            {
                //processor.Execute(_cancelSource.Token);
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled exception from processor.Execute method", ex);
                throw;
            }
        }

        public override void OnStop()
        {
            UnsubscribeServiceBusMessageBrokers();

            _cancelSource.Cancel();

            base.OnStop();
        }

        private void Initialise()
        {
            try
            {
                VersionLogging.SetVersion();

                InitializeIoC();
                InitialiseServiceBus();
                SubscribeServiceBusMessageBrokers();
            }
            catch (Exception e)
            {
                if (_logService == null)
                {
                    Trace.TraceError($"{ProcessName} failed to initialise: \"{e}\"");
                }
                else
                {
                    _logService.Error($"{ProcessName} failed to initialise");
                }

                throw;
            }
        }

        private void InitializeIoC()
        {
            var container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration()));
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();

            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry<FaaMigrationRegistry>();
            });

            _logService = _container.GetInstance<ILogService>();
        }

        private void InitialiseServiceBus()
        {
            _logService.Debug("Initialising service bus");

            _container.GetInstance<IServiceBusInitialiser>().Initialise();

            _logService.Debug("Initialised service bus");
        }

        private void SubscribeServiceBusMessageBrokers()
        {
            _logService.Debug("Subscribing service bus message brokers");

            var brokers = _container.GetAllInstances<IServiceBusMessageBroker>();

            var count = 0;

            foreach (var broker in brokers)
            {
                broker.Subscribe();
                count++;
            }

            _logService.Debug("Subscribed {0} service bus message broker(s)", count);
        }

        private void UnsubscribeServiceBusMessageBrokers()
        {
            _logService.Debug("Unsubscribing service bus message brokers");

            var brokers = _container.GetAllInstances<IServiceBusMessageBroker>();

            var count = 0;

            foreach (var broker in brokers)
            {
                broker.Unsubscribe();
                count++;
            }

            _logService.Debug("Unsubscribed {0} service bus message broker(s)", count);
        }
    }
}