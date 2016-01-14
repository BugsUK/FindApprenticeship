namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Services
{
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using SFA.Infrastructure.Interfaces;
    using Nest;
    using Configuration;

    public class LogEventIndexerService : ILogEventIndexerService
    {
        private IConfigurationService _configurationService;

        public LogEventIndexerService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Index(string logEvent)
        {
            try
            {
                var indexDate = GetIndexDate(logEvent);
                var indexName = GetIndexName(indexDate);
                var client = GetElasticClient();

                var response = client.Raw.Index(indexName, "logEvent", logEvent);

                if (!response.Success)
                {
                    Trace.TraceError("Elasticsearch failed to index log event: \"{0}\" (HttpStatusCode: {1})",
                        logEvent, response.HttpStatusCode);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception occurred when attempting to index log event: \"{0}\"", e.ToString());
            }
        }

        #region Helpers

        private ElasticClient GetElasticClient()
        {
            var configuration = _configurationService.Get<AzureEventHubLogIndexerConfiguration>();
            var connectionSettings = new ConnectionSettings(new Uri(configuration.ElasticsearchHostName));

            return new ElasticClient(connectionSettings);
        }

        private static DateTime GetIndexDate(string logEvent)
        {
            try
            {
                var logEventObject = JsonConvert.DeserializeObject<dynamic>(logEvent);

                // NOTE: logged date is lower case.
                return Convert.ToDateTime(logEventObject.date);
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        private static string GetIndexName(DateTime dateTime)
        {
            const string indexNamePrefix = "log";
            const string dateSuffixFormat = "yyyy.MM.dd";

            return string.Format("{0}-{1}",
                indexNamePrefix, dateTime.ToUniversalTime().ToString(dateSuffixFormat));
        }

        #endregion
    }
}