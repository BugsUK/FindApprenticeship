namespace SFA.Apprenticeships.Infrastructure.Processes
{
    using System;
    using System.Net;
    using System.Threading;
    using Application.Candidates;
    using Azure.Common.IoC;
    using Azure.ServiceBus;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Communication.IoC;
    using Elastic.Common.IoC;
    using EmployerDataService.IoC;
    using IoC;
    using LocationLookup.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Postcode.IoC;
    using Raa.IoC;
    using Repositories.Mongo.Applications.IoC;
    using Repositories.Mongo.Audit.IoC;
    using Repositories.Mongo.Authentication.IoC;
    using Repositories.Mongo.Candidates.IoC;
    using Repositories.Mongo.Communication.IoC;
    using Repositories.Mongo.Users.IoC;
    using Repositories.Sql.Configuration;
    using Repositories.Sql.IoC;
    using Repositories.Sql.Schemas.Vacancy.IoC;
    using Application.Interfaces;
    using Repositories.Sql.Schemas.Employer.IoC;
    using Repositories.Sql.Schemas.Provider.IoC;
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
                x.AddRegistry(new CommonRegistry(new CacheConfiguration()));
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            _container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<AuditRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddCachingRegistry(cacheConfig);
                x.AddRegistry<RaaRegistry>();
                x.AddRegistry<VacancySourceRegistry>();
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
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry<EmployerRepositoryRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));
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