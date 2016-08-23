namespace SFA.Apprenticeships.Infrastructure.Monitor.Provider
{
    using SFA.Infrastructure.Interfaces;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;

    using SFA.Apprenticeships.Application.Interfaces;

    public class VacancyMetricsProvider : IVacancyMetricsProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly ILogService _logService;

        public VacancyMetricsProvider(IElasticsearchClientFactory elasticsearchClientFactory, ILogService logService)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _logService = logService;
        }

        public long GetApprenticeshipsCount()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(ApprenticeshipSummary));

            var response = client.Count<ApprenticeshipSummary>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);

                return s;
            });

            var count = response.Count;

            return count;
        }

        public long GetTraineeshipsCount()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(TraineeshipSummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(TraineeshipSummary));

            var response = client.Count<ApprenticeshipSummary>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);

                return s;
            });

            var count = response.Count;

            return count;
        }
    }
}