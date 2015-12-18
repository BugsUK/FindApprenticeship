namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck
{
    using System;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using Elastic.Common.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging.IoC;
    using StructureMap;
    using Tasks;
    using VacancySearch.IoC;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();

            container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<AzureServiceBusRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig, new ServicesConfiguration {ServiceImplementation = "Legacy"}));
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<MessageLogCheckRepository>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            var messageLossCheckTaskRunner = container.GetInstance<IMessageLossCheckTaskRunner>();

            messageLossCheckTaskRunner.RunMonitorTasks();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
