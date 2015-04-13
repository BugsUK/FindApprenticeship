﻿namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using System.Collections.Generic;
    using Application.Interfaces.Logging;

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