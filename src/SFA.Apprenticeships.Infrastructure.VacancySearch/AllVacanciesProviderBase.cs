namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancy;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Elasticsearch.Net;
    using Nest;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class AllVacanciesProviderBase<TVacancySummary> : IAllVacanciesProvider
        where TVacancySummary : class, IVacancySummary
    {
        private const string ScrollIndexConsistencyTime = "5s";
        private const int ScrollSize = 100;
        private const string ScrollTimeout = "5s";

        private readonly ILogService _logger;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        protected AllVacanciesProviderBase(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _logger = logger;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<int> GetAllVacancyIds(string indexName)
        {
            _logger.Debug("Getting all vacancy id from index '{0}' with ScrollSize='{1}', ScrollIndexConsistencyTime='{2}', ScrollTimeout='{3}'",
                indexName, ScrollSize, ScrollIndexConsistencyTime, ScrollTimeout);

            var client = _elasticsearchClientFactory.GetElasticClient();
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(TVacancySummary));

            // REFERENCE: http://nest.azurewebsites.net/nest/search/scroll.html.
            var scanResults = client.Search<TVacancySummary>(search => search
                .Index(indexName)
                .Type(documentTypeName)
                .From(0)
                .Size(ScrollSize)
                .MatchAll()
                .SearchType(SearchType.Scan)
                .Scroll(ScrollIndexConsistencyTime));

            var vacancies = new List<int>();
            var scrollRequest = new ScrollRequest(scanResults.ScrollId, ScrollTimeout);
            var scrollCount = 0;

            while (true)
            {
                var results = client.Scroll<TVacancySummary>(scrollRequest);

                scrollCount++;

                if (!results.Documents.Any())
                {
                    break;
                }

                vacancies.AddRange(results.Documents.Select(each => each.Id));
            }

            _logger.Debug("Got {0} vacancy ids from index '{1}' in {2} 'scrolls'", vacancies.Count, indexName, scrollCount);
            
            return vacancies;
        }
    }
}