namespace SFA.Apprenticeships.Metrics.Candidate
{
    using System;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using IoC;
    using StructureMap;
    using Tasks;

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
                x.AddRegistry<MetricsRepository>();
            });

            var messageLossCheckTaskRunner = container.GetInstance<IMetricsTaskRunner>();

            messageLossCheckTaskRunner.RunMetricsTasks();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
