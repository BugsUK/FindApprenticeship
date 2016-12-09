namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Common.IoC;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Communication.Configuration;
    using Consumers;
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
    using Repositories.Mongo.Candidates.IoC;
    using Repositories.Mongo.Communication.IoC;
    using Repositories.Mongo.Employers.IoC;
    using Repositories.Mongo.Users.IoC;
    using Repositories.Sql.Configuration;
    using Repositories.Sql.IoC;
    using Repositories.Sql.Schemas.Vacancy.IoC;
    using Application.Interfaces;
    using Repositories.Sql.Schemas.Provider.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Jobs Processor";
        private VacancyEtlControlQueueConsumer _vacancyEtlControlQueueConsumer;
        private SavedSearchControlQueueConsumer _savedSearchControlQueueConsumer;
        private DailyDigestControlQueueConsumer _dailyDigestControlQueueConsumer;
        private HousekeepingControlQueueConsumer _housekeepingControlQueueConsumer;
        private VacancyStatusControlQueueConsumer _vacancyStatusControlQueueConsumer;
        private IConfigurationService _configurationService;
        private IContainer _container;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    var tasks = new List<Task>
                    {
                        _savedSearchControlQueueConsumer.CheckScheduleQueue(),
                        _vacancyEtlControlQueueConsumer.CheckScheduleQueue(),
                        _housekeepingControlQueueConsumer.CheckScheduleQueue(),
                        _vacancyStatusControlQueueConsumer.CheckScheduleQueue()
                    };

                    if (CommunicationsIsEnabled)
                    {
                        _logger.Debug("Communications job is enabled");
                        tasks.Add(_dailyDigestControlQueueConsumer.CheckScheduleQueue());
                    }
                    else
                    {
                        _logger.Debug("Communications job is disabled");
                    }

                    Task.WaitAll(tasks.ToArray());
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

            // ReSharper disable once FunctionNeverReturns
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

        private bool CommunicationsIsEnabled
        {
            get
            {
                return _configurationService.Get<CommunicationConfiguration>().IsEnabled;
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
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            _container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddCachingRegistry(cacheConfig);
                x.AddRegistry<RaaRegistry>();
                x.AddRegistry<VacancySourceRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<JobsRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry<EmployerRepositoryRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));
            });

            _logger = _container.GetInstance<ILogService>();
            _vacancyEtlControlQueueConsumer = _container.GetInstance<VacancyEtlControlQueueConsumer>();
            _savedSearchControlQueueConsumer = _container.GetInstance<SavedSearchControlQueueConsumer>();
            _dailyDigestControlQueueConsumer = _container.GetInstance<DailyDigestControlQueueConsumer>();
            _housekeepingControlQueueConsumer = _container.GetInstance<HousekeepingControlQueueConsumer>();
            _vacancyStatusControlQueueConsumer = _container.GetInstance<VacancyStatusControlQueueConsumer>();
            _configurationService = _container.GetInstance<IConfigurationService>();
        }
    }
}
