﻿namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck
{
    using System;
    using Application.ReferenceData.Configuration;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using Elastic.Common.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging.IoC;
    using RabbitMq.IoC;
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
            var referenceDataConfiguration = configurationService.Get<ReferenceDataConfiguration>();

            container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig, referenceDataConfiguration));
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<MessageLogCheckRepository>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });

            var messageLossCheckTaskRunner = container.GetInstance<IMessageLossCheckTaskRunner>();

            messageLossCheckTaskRunner.RunMonitorTasks();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            container.GetInstance<IBus>().Advanced.Dispose();
        }
    }
}
