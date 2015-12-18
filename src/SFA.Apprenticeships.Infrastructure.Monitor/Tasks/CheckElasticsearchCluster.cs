﻿namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Elastic.Common.Configuration;
    using Elasticsearch.Net;
    using Nest;

    internal class CheckElasticsearchCluster : IMonitorTask
    {
        private readonly ILogService _logger;

        private readonly SearchConfiguration _elasticsearchConfiguration;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public CheckElasticsearchCluster(IElasticsearchClientFactory elasticsearchClientFactory, ILogService logger, IConfigurationService configurationService)
        {
            _elasticsearchConfiguration = configurationService.Get<SearchConfiguration>();
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Elasticsearch Cluster"; }
        }

        public void Run()
        {
            var health = GetClusterHealth();

            EnsureNoTimeout(health);
            EnsureExpectedNumberOfNodes(health);
            EnsureClusterIsHealthy(health);
        }

        private IHealthResponse GetClusterHealth()
        {
            var request = new ClusterHealthRequest
            {
                Level = Level.Cluster,
                //http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-health.html
                WaitForStatus = WaitForStatus.Yellow,
                Timeout = Timeout
            };

            var client = _elasticsearchClientFactory.GetElasticClient();

            return client.ClusterHealth(request);
        }

        private void EnsureNoTimeout(IHealthResponse health)
        {
            if (!health.TimedOut)
            {
                return;
            }

            var message = string.Format(
                "Elasticsearch cluster health check timed out ({0}).", Timeout);

            throw new Exception(message);
        }

        private void EnsureExpectedNumberOfNodes(IHealthResponse response)
        {
            if (ExpectedNodeCount == response.NumberOfNodes)
            {
                return;
            }

            var message = string.Format(
                "Expected {0} Elasticsearch node(s), saw {1}.", ExpectedNodeCount, response.NumberOfNodes);

            throw new Exception(message);
        }

        private void EnsureClusterIsHealthy(IHealthResponse health)
        {
            if (health.Status == "green")
            {
                return;
            }

            //Clusters with only one node allways have status of "yellow"
            if ((health.Status == "yellow" && ExpectedNodeCount > 1) || health.Status == "red")
            {
                var statusMessage = string.Format("Cluster is unhealthy: \"{0}\". Advise checking cluster if this message is logged again.", health.Status);
                _logger.Warn(statusMessage);
            }

            if (health.NumberOfNodes != ExpectedNodeCount)
            {
                var message = string.Format("Cluster should contain {0} nodes, but only has {1}.", ExpectedNodeCount, health.NumberOfNodes);
                _logger.Warn(message);
            }
        }

        private int ExpectedNodeCount
        {
            get { return _elasticsearchConfiguration.NodeCount; }
        }

        private string Timeout
        {
            get { return string.Format("{0}s", _elasticsearchConfiguration.Timeout); }
        }
    }
}
