namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Services
{
    using System;
    using System.Diagnostics;
    using Domain;
    using Elastic.Common.Configuration;
    using Nest;
    using Newtonsoft.Json;

    public class LogEventIndexerService : ILogEventIndexerService
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public LogEventIndexerService(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public void Index(string logEvent)
        {
            try
            {
                var indexDate = GetIndexDate(logEvent);
                var indexName = GetIndexName(indexDate);
                var client = _elasticsearchClientFactory.GetElasticClient();

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

        private static DateTime GetIndexDate(string logEvent)
        {
            try
            {
                var logEventObject = JsonConvert.DeserializeObject<dynamic>(logEvent);

                return Convert.ToDateTime(logEventObject.Date);
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        private static string GetIndexName(DateTime dateTime)
        {
            const string indexNamePrefix = "log";
            const string dateSuffixFormat = "yyyy-MM-dd-HH";

            return string.Format("{0}.{1}",
                indexNamePrefix, dateTime.ToUniversalTime().ToString(dateSuffixFormat));
        }
    }
}