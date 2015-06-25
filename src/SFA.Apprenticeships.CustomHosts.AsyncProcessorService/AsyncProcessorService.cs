namespace SFA.Apprenticeships.CustomHosts.AsyncProcessorService
{
    using System.Net;
    using System.Threading;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using Infrastructure.Azure.Common.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Communication.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Processes.Communications;
    using Infrastructure.Processes.IoC;
    using Infrastructure.RabbitMq.Interfaces;
    using Infrastructure.RabbitMq.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using Application.ReferenceData.Configuration;
    using Infrastructure.Repositories.Communication.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.VacancyIndexer.IoC;
    using Infrastructure.VacancySearch.IoC;
    using StructureMap;

    public partial class AsyncProcessorService : ServiceBase
    {
        private StructureMap.IContainer _container;

        public AsyncProcessorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            Initialise();
        }

        protected override void OnStop()
        {
            // Kill the bus which will kill any subscriptions
            _container.GetInstance<IBus>().Advanced.Dispose();
            
            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private void Initialise()
        {
            InitializeIoC();
            InitializeRabbitMQSubscribers();
        }

        private void InitializeRabbitMQSubscribers()
        {
            const string asyncProcessorSubscriptionId = "AsyncProcessor";

            var bootstrapper = _container.GetInstance<IBootstrapSubcribers>();

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(EmailRequestConsumerAsync)), asyncProcessorSubscriptionId, _container);
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
            var referenceDataConfiguration = configurationService.Get<ReferenceDataConfiguration>();

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
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig, referenceDataConfiguration));
                x.AddRegistry<ProcessesRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
            });
        }

        public void RunConsole()
        {
            OnStart(null);

            Console.WriteLine("AsyncProcessorService is running… Press any key to stop");

            Console.ReadKey();

            OnStop();
            Environment.Exit(1);
        }
    }
}
