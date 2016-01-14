namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using Elastic.Common.Configuration;
    using Elasticsearch.Net;
    using Nest;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Deserializers;

    /// <summary>
    /// Checks that Logstash log entries are being created.
    /// </summary>
    internal class CheckLogstashLogs : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly MonitorConfiguration _monitorConfiguration;
        private readonly LogstashConfiguration _logstashConfiguration;

        public class DynamicJsonDeserializer : IDeserializer
        {
            public string RootElement { get; set; }
            public string Namespace { get; set; }
            public string DateFormat { get; set; }

            public T Deserialize<T>(IRestResponse response)
            {
                return JsonConvert.DeserializeObject<dynamic>(response.Content);
            }
        }

        public CheckLogstashLogs(IConfigurationService configurationService, ILogService logger)
        {
            _monitorConfiguration = configurationService.Get<MonitorConfiguration>();
            _logstashConfiguration = configurationService.Get<LogstashConfiguration>();
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Logstash Logs"; }
        }

        public void Run()
        {
            EnsureClusterIsHealthy();
            EnsureExpectedNumberOfMessagesLoggedInTimeframe();            
        }

        #region Querying Logstash Tests

        private void EnsureExpectedNumberOfMessagesLoggedInTimeframe()
        {
            var timeframeStart = DateTime.UtcNow.AddMinutes(-_monitorConfiguration.ExpectedLogTimeframeInMinutes);

            var timestamps = GetRecentLogEntryTimestamps();
            var actualLogCount = timestamps.Count(timestamp => timestamp >= timeframeStart);

            _logger.Debug("Looking for {0} Logstash message(s) in last {1} minute(s), saw {2}.",
                _monitorConfiguration.ExpectedMinimumLogCount, _monitorConfiguration.ExpectedLogTimeframeInMinutes, actualLogCount);

            if (actualLogCount >= _monitorConfiguration.ExpectedMinimumLogCount)
            {
                return;
            }

            var message = string.Format(
                "Expected {0} Logstash message(s) in last {1} minute(s), saw {2}.",
                _monitorConfiguration.ExpectedMinimumLogCount, _monitorConfiguration.ExpectedLogTimeframeInMinutes, actualLogCount);

            throw new Exception(message);
        }

        private IEnumerable<DateTime> GetRecentLogEntryTimestamps()
        {
            var client = CreateRestClient();
            var timestamps = new List<DateTime>();

            var indexDate = DateTime.UtcNow.AddMinutes(-30);
            var uri = BuildUri(indexDate);
            var request = CreateRestRequest(uri);

            var response = client.Execute<dynamic>(request);
            if (response == null)
            {
                throw new Exception("Logstash query returned a null response");
            }
                
            EnsureResponseStatusCodeIsOk(response);
            EnsureResponseHasData(response);

            var logEntries = response.Data.hits.hits;

            foreach (var logEntry in logEntries)
            {
                // It's whacky but the JSON 'path' to a timestamp value looks like this:
                // response.Data.hits.hits[0].fields["@timestamp"][0].Value
                timestamps.Add(logEntry.fields["@timestamp"][0].Value);
            }

            return timestamps;
        }

        private static void EnsureResponseHasData(IRestResponse<dynamic> response)
        {
            if (response.Data == null || response.Data.hits == null || response.Data.hits.hits == null)
            {
                throw new Exception("Logstash query returned no log entry data.");
            }
        }

        private static void EnsureResponseStatusCodeIsOk(IRestResponse<dynamic> response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = string.Format("Logstash query returned HTTP status code {0}.", response.StatusCode);

                throw new Exception(message);
            }
        }

        private RestClient CreateRestClient()
        {
            var client = new RestClient(_logstashConfiguration.HostName);

            client.AddHandler("application/json", new DynamicJsonDeserializer());

            return client;
        }

        public virtual IRestRequest CreateRestRequest(string uri)
        {
            var request = new RestRequest(uri, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                Timeout = _logstashConfiguration.Timeout * 1000
            };

            request.AddHeader("Accept", "application/json");

            return request;
        }

        private string BuildUri(DateTime indexDate)
        {
            const string format = "logstash-{0}/logstash/_search?sort=@timestamp:desc&fields=@timestamp&size={1}";
            var uri = string.Format(format, indexDate.ToString("yyyy.MM.dd"), _monitorConfiguration.ExpectedMinimumLogCount);

            return uri;
        }

        #endregion

        #region Query Elasticsearch Management API Tests

        private void EnsureClusterIsHealthy()
        {
            var health = GetClusterHealth();
            EnsureNoTimeout(health);
            EnsureExpectedNumberOfNodes(health);
            EnsureClusterIsHealthy(health);
        }

        private void EnsureNoTimeout(IHealthResponse health)
        {
            if (!health.TimedOut)
            {
                return;
            }

            var message = string.Format("Logstash elastic cluster health check timed out ({0}).", _logstashConfiguration.Timeout);

            throw new Exception(message);
        }

        private void EnsureExpectedNumberOfNodes(IHealthResponse response)
        {
            if (_logstashConfiguration.NodeCount == response.NumberOfNodes) { return; }
            var message = string.Format("Expected {0} Elasticsearch node(s), saw {1}.", _logstashConfiguration.NodeCount, response.NumberOfNodes);
            throw new Exception(message);
        }

        private void EnsureClusterIsHealthy(IHealthResponse health)
        {
            if (health.Status == "green")
            {
                return;
            }

            //Clusters with only one node allways have status of "yellow"
            if ((health.Status == "yellow" && _logstashConfiguration.NodeCount > 1) || health.Status == "red")
            {
                var statusMessage = string.Format("Cluster is unhealthy: \"{0}\". Advise checking cluster if this message is logged again.", health.Status);
                _logger.Warn(statusMessage);
            }

            if (health.NumberOfNodes != _logstashConfiguration.NodeCount)
            {
                var message = string.Format("Cluster should contain {0} nodes, but only has {1}.", _logstashConfiguration.NodeCount, health.NumberOfNodes);
                _logger.Warn(message);
            }
        }

        private IHealthResponse GetClusterHealth()
        {
            var request = new ClusterHealthRequest
            {
                Level = Level.Cluster,
                //http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-health.html
                WaitForStatus = WaitForStatus.Yellow,
                Timeout = _logstashConfiguration.Timeout.ToString()
            };

            var client = new ElasticClient(new ConnectionSettings(new Uri(_logstashConfiguration.HostName)));
            return client.ClusterHealth(request);
        }

        #endregion
    }
}