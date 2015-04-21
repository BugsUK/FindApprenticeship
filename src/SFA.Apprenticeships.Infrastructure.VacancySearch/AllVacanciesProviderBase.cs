namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elasticsearch.Net;
    using Nest;

    // TODO: AG: US438: logging.
    public abstract class AllVacanciesProviderBase<TVacancySearchResponse, TVacancySummary> : IAllVacanciesProvider<TVacancySearchResponse>
        where TVacancySearchResponse : class
        where TVacancySummary : class
    {
        // TODO: AG: US438: consider moving to configuration.
        private const string ScrollIndexConsistencyTime = "2s";
        private const int ScrollSize = 100;
        private const string ScrollTimeout = "5s";

        private readonly ILogService _logger;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _vacancySearchMapper;

        protected AllVacanciesProviderBase(
            ILogService logger,
            IElasticsearchClientFactory elasticsearchClientFactory,
            IMapper vacancySearchMapper)
        {
            _logger = logger;
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _vacancySearchMapper = vacancySearchMapper;
        }

        public IEnumerable<TVacancySearchResponse> GetAllVacancies()
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(TVacancySummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(TVacancySummary));

            // REFERENCE: http://nest.azurewebsites.net/nest/search/scroll.html.
            var scanResults = client.Search<ApprenticeshipSummary>(search => search
                .Index(indexName)
                .Type(documentTypeName)
                .From(0)
                .Size(ScrollSize)
                .MatchAll()
                .SearchType(SearchType.Scan)
                .Scroll(ScrollIndexConsistencyTime));

            var vacancies = new List<TVacancySearchResponse>();

            while (true)
            {
                var scrollRequest = new ScrollRequest(scanResults.ScrollId, ScrollTimeout);
                var results = client.Scroll<TVacancySummary>(scrollRequest);

                if (!results.Documents.Any())
                {
                    break;
                }
    
                vacancies.AddRange(_vacancySearchMapper
                    .Map<IEnumerable<TVacancySummary>, IEnumerable<TVacancySearchResponse>>(results.Documents));
            }

            return vacancies;
        }
    }
}