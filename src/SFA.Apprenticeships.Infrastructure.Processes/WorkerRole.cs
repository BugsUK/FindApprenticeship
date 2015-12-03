namespace SFA.Apprenticeships.Infrastructure.Processes
{
    using System;
    using System.Net;
    using System.Threading;
    using Application.Candidates;
    using Application.Interfaces.Logging;
    using Azure.Common.IoC;
    using Azure.ServiceBus;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Communication.IoC;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using LocationLookup.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Postcode.IoC;
    using Raa.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Audit.IoC;
    using Repositories.Authentication.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using Repositories.Users.IoC;
    using Repositories.Vacancies.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Background Processor";
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private IContainer _container;

        public override void Run()
        {
            Initialise();

            _cancelSource.Token.WaitHandle.WaitOne();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            UnsubscribeServiceBusMessageBrokers();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            _cancelSource.Cancel();

            base.OnStop();
        }

        private void Initialise()
        {
            VersionLogging.SetVersion();

            try
            {
                InitializeIoC();
                InitialiseServiceBus();
                SubscribeServiceBusMessageBrokers();
            }
            catch (Exception ex)
            {
                if (_logger != null) _logger.Error(ProcessName + " failed to initialise", ex);
                throw;
            }
        }

        private void InitializeIoC()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();
            var servicesConfiguration = configurationService.Get<ServicesConfiguration>();

            _container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<AzureServiceBusRegistry>();
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<AuditRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddCachingRegistry(cacheConfig);
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig, servicesConfiguration));
                x.AddRegistry(new RaaRegistry(servicesConfiguration));
                x.AddRegistry<ProcessesRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
                x.Scan(y =>
                {
                    y.AssemblyContainingType<IHousekeepingChainOfResponsibility>();
                    y.AddAllTypesOf<IHousekeepingChainOfResponsibility>();
                });
                x.AddRegistry<AuthenticationRepositoryRegistry>();
            });

            _logger = _container.GetInstance<ILogService>();
        }

        private void InitialiseServiceBus()
        {
            _logger.Debug("Initialising service bus");

            _container.GetInstance<IServiceBusInitialiser>().Initialise();

            _logger.Debug("Initialised service bus");
        }

        private void SubscribeServiceBusMessageBrokers()
        {
            _logger.Debug("Subscribing service bus message brokers");

            var brokers = _container.GetAllInstances<IServiceBusMessageBroker>();

            var count = 0;

            foreach (var broker in brokers)
            {
                broker.Subscribe();
                count++;
            }

            _logger.Debug("Subscribed {0} service bus message broker(s)", count);
        }

        private void UnsubscribeServiceBusMessageBrokers()
        {
            _logger.Debug("Unsubscribing service bus message brokers");

            var brokers = _container.GetAllInstances<IServiceBusMessageBroker>();

            var count = 0;

            foreach (var broker in brokers)
            {
                broker.Unsubscribe();
                count++;
            }

            _logger.Debug("Unsubscribed {0} service bus message broker(s)", count);
        }
    }
}