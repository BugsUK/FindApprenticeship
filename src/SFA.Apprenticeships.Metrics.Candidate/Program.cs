namespace SFA.Apprenticeships.Metrics.Candidate
{
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
                x.AddRegistry<MetricsRepository>();
            });

            var messageLossCheckTaskRunner = container.GetInstance<IMetricsTaskRunner>();

            messageLossCheckTaskRunner.RunMetricsTasks();
        }
    }
}
