namespace SFA.Apprenticeships.Infrastructure.VacancyEtl
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Application.Interfaces.Logging;
    using Azure.Common.IoC;
    using Caching.Azure.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Consumers;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
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
        private const string ProcessName = "Vacancy Processor";
        private VacancyEtlControlQueueConsumer _vacancyEtlControlQueueConsumer;
        private SavedSearchControlQueueConsumer _savedSearchControlQueueConsumer;
        private IContainer _container; 

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    _vacancyEtlControlQueueConsumer.CheckScheduleQueue().Wait();
                    _savedSearchControlQueueConsumer.CheckScheduleQueue().Wait();
                }
                catch (FaultException fe)
                {
                    _logger.Error("FaultException from  " + ProcessName, fe);
                }
                catch (CommunicationException ce)
                {
                    _logger.Warn("CommunicationException from " + ProcessName, ce);
                }
                catch (TimeoutException te)
                {
                    _logger.Warn("TimeoutException from  " + ProcessName, te);
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception from  " + ProcessName, ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        private void Initialise()
        {
            VersionLogging.SetVersion();

            try
            {
                InitializeIoC();
            }
            catch (Exception ex)
            {
                if (_logger != null) _logger.Error(ProcessName + " failed to initialise", ex);
                throw;
            }
        }

        private void InitializeIoC()
        {
            var config = new ConfigurationManager();
            var useCacheSetting = config.TryGetAppSetting("UseCaching");
            bool useCache;
            bool.TryParse(useCacheSetting, out useCache);

            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<AzureCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<VacancyEtlRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            _logger = _container.GetInstance<ILogService>();
            _vacancyEtlControlQueueConsumer = _container.GetInstance<VacancyEtlControlQueueConsumer>();
            _savedSearchControlQueueConsumer = _container.GetInstance<SavedSearchControlQueueConsumer>();
        }
    }
}
