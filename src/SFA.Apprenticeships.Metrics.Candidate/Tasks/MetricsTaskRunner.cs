namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using System.Collections.Generic;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class MetricsTaskRunner : IMetricsTaskRunner
    {
        private readonly ILogService _logger;
        private readonly IEnumerable<IMetricsTask> _metricsTasks;

        public MetricsTaskRunner(IEnumerable<IMetricsTask> metricsTasks, ILogService logger)
        {
            _metricsTasks = metricsTasks;
            _logger = logger;
        }

        public void RunMetricsTasks()
        {
            foreach (var metricsTask in _metricsTasks)
            {
                _logger.Info("Running " + metricsTask.TaskName);
                metricsTask.Run();
            }
        }
    }
}