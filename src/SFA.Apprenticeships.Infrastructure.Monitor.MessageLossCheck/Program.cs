namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck
{
    using System;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using Elastic.Common.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Infrastructure.Repositories.Mongo.Candidates.IoC;
    using Infrastructure.Repositories.Mongo.Users.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging.IoC;

    using SFA.Apprenticeships.Application.Candidate.Configuration;
    using SFA.Apprenticeships.Application.Interfaces;

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
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();

            container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry(
                    new LegacyWebServicesRegistry(
                        new ServicesConfiguration
                        {
                            ServiceImplementation = ServicesConfiguration.Legacy,
                            VacanciesSource = ServicesConfiguration.Legacy
                        }, new CacheConfiguration()));
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(),
                    new ServicesConfiguration
                    {
                        ServiceImplementation = ServicesConfiguration.Legacy,
                        VacanciesSource = ServicesConfiguration.Legacy
                    }));
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
