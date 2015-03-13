namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl
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
    using IoC;
    using LegacyWebServices.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Application Processor";
        private ApplicationEtlControlQueueConsumer _applicationEtlControlQueueConsumer;
        private IContainer _container;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    _applicationEtlControlQueueConsumer.CheckScheduleQueue().Wait();
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
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<AzureCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationEtlRegistry>();
            });

            _logger = _container.GetInstance<ILogService>();
            _applicationEtlControlQueueConsumer = _container.GetInstance<ApplicationEtlControlQueueConsumer>();
        }
    }
}
