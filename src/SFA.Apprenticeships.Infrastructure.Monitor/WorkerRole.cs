namespace SFA.Apprenticeships.Infrastructure.Monitor
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Common.IoC;
    using Azure.ServiceBus.Configuration;
    using Azure.ServiceBus.IoC;
    using Common.IoC;
    using Consumers;
    using Elastic.Common.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Infrastructure.Repositories.Mongo.Audit.IoC;
    using Infrastructure.Repositories.Mongo.Authentication.IoC;
    using Infrastructure.Repositories.Mongo.Candidates.IoC;
    using Infrastructure.Repositories.Mongo.Users.IoC;
    using IoC;
    using LocationLookup.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Postcode.IoC;
    using Application.Interfaces;
    using StructureMap;
    using UserDirectory.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private const string ProcessName = "Monitor Process";

        private static ILogService _logger;

        private IContainer _container;

        private MonitorControlQueueConsumer _monitorControlQueueConsumer;
        private DailyMetricsControlQueueConsumer _dailyMetricsControlQueueConsumer;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    var tasks = new[]
                    {
                        _monitorControlQueueConsumer.CheckScheduleQueue(),
                        _dailyMetricsControlQueueConsumer.CheckScheduleQueue()
                    };

                    Task.WaitAll(tasks);
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

        public override void OnStop()
        {
            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }

        private void Initialise()
        {
            VersionLogging.SetVersion();

            try
            {
                InitializeIoC();
                InitialiseSubscribers();
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
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();

            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry<VacancySourceRegistry>();
                x.AddRegistry<MonitorRegistry>();
                x.AddRegistry<AuditRepositoryRegistry>();
            });

            _logger = _container.GetInstance<ILogService>();
        }

        private void InitialiseSubscribers()
        {
            _monitorControlQueueConsumer = _container.GetInstance<MonitorControlQueueConsumer>();
            _dailyMetricsControlQueueConsumer = _container.GetInstance<DailyMetricsControlQueueConsumer>();
        }
    }
}
