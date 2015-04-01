namespace SFA.Apprenticeships.Infrastructure.Processes
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using Application.Interfaces.Logging;
    using Azure.Common.IoC;
    using Caching.Azure.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Communication.IoC;
    using Communications;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using LocationLookup.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Postcode.IoC;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Background Processor";
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private StructureMap.IContainer _container;

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
            // Kill the bus which will kill any subscriptions
            _container.GetInstance<IBus>().Advanced.Dispose();

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
                InitialiseRabbitMQSubscribers();
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

            _container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddCachingRegistry(cacheConfig);
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig));
                x.AddRegistry<ProcessesRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
            });

            _logger = _container.GetInstance<ILogService>();
        }

        private void InitialiseRabbitMQSubscribers()
        {
            var bootstrapper = _container.GetInstance<IBootstrapSubcribers>();

            _logger.Debug("RabbitMQ initialising");

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(EmailRequestConsumerAsync)), "AsyncProcessor", _container);

            _logger.Debug("RabbitMQ initialised");
        }
    }
}
