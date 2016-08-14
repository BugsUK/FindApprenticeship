namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies.Entities;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Nest;

    using Application.Candidate.Configuration;

    public class VacancyIndexerService<TSourceSummary, TDestinationSummary> : IVacancyIndexerService<TSourceSummary, TDestinationSummary>
        where TSourceSummary : Domain.Entities.Vacancies.VacancySummary, IVacancyUpdate
        where TDestinationSummary : class, IVacancySummary
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private const int PageSize = 5;

        public VacancyIndexerService(
            ILogService logger,
            IConfigurationService configurationService,
            IMapper mapper,
            IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _logger = logger;
            _configurationService = configurationService;
            _mapper = mapper;
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public void Index(TSourceSummary vacancySummaryToIndex)
        {
            _logger.Debug("Indexing vacancy item : {0} ({1})", vacancySummaryToIndex.Title, vacancySummaryToIndex.Id);

            try
            {
                var indexAlias = GetIndexAlias();
                var newIndexName = GetIndexNameAndDateExtension(indexAlias, vacancySummaryToIndex.ScheduledRefreshDateTime, vacancySummaryToIndex.UseAlias);
                var vacancySummaryElastic = _mapper.Map<TSourceSummary, TDestinationSummary>(vacancySummaryToIndex);

                var client = _elasticsearchClientFactory.GetElasticClient();
                var indexResponse = client.Index(vacancySummaryElastic, f => f.Index(newIndexName));
                if (!indexResponse.ConnectionStatus.Success)
                {
                    _logger.Info("Failed to index vacancy item : {0} ({1}). Response {2} {3}", vacancySummaryToIndex.Title, vacancySummaryToIndex.Id, indexResponse.ConnectionStatus.HttpStatusCode, indexResponse.ConnectionStatus);
                }
                else
                {
                    _logger.Debug("Indexed vacancy item : {0} ({1})", vacancySummaryToIndex.Title, vacancySummaryToIndex.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed indexing traineeship vacancy summary {0}", ex, vacancySummaryToIndex.Id);
            }
        }

        public void CreateScheduledIndex(DateTime scheduledRefreshDateTime)
        {
            _logger.Info("Creating new vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime, false);
            var client = _elasticsearchClientFactory.GetElasticClient();

            var indexExistsResponse = client.IndexExists(i => i.Index(newIndexName));

            if (indexExistsResponse.Exists)
            {
                // If it already exists and is empty, let's delete it and recreate it.
                var totalResults = client.Count<TDestinationSummary>(c =>
                {
                    c.Index(newIndexName);
                    return c;
                });

                if (totalResults.Count == 0)
                {
                    client.DeleteIndex(i => i.Index(newIndexName));
                    indexExistsResponse = client.IndexExists(i => i.Index(newIndexName));
                }
            }

            if (!indexExistsResponse.Exists)
            {
                var indexSettings = new IndexSettings();
                var searchConfiguration = _configurationService.Get<SearchConfiguration>();
                var synonyms = searchConfiguration.Synonyms.ToArray();

                if (synonyms.Any())
                {
                    var synonymFilter = new SynonymTokenFilter
                    {
                        Synonyms = synonyms
                    };

                    indexSettings.Analysis.TokenFilters.Add("synonym", synonymFilter);
                }

                //Token filters
                var snowballTokenFilter = new SnowballTokenFilter { Language = "English" };
                indexSettings.Analysis.TokenFilters.Add("snowball", snowballTokenFilter);

                var baseStopwords = searchConfiguration.StopwordsBase.ToList();
                var extendedStopwords = baseStopwords.Concat(searchConfiguration.StopwordsExtended);

                var stopwordsBaseFilter = new StopTokenFilter { Stopwords = baseStopwords };
                indexSettings.Analysis.TokenFilters.Add("stopwordsBaseFilter", stopwordsBaseFilter);

                var stopwordsExtendedFilter = new StopTokenFilter { Stopwords = extendedStopwords };
                indexSettings.Analysis.TokenFilters.Add("stopwordsExtendedFilter", stopwordsExtendedFilter);

                //Analysers
                indexSettings.Analysis.Analyzers.Add("snowballStopwordsBase", new CustomAnalyzer { Tokenizer = "standard", Filter = new[] { "standard", "lowercase", "stopwordsBaseFilter", "synonym", "snowball" } });
                indexSettings.Analysis.Analyzers.Add("snowballStopwordsExtended", new CustomAnalyzer { Tokenizer = "standard", Filter = new[] { "standard", "lowercase", "stopwordsExtendedFilter", "synonym", "snowball" } });

                indexSettings.Analysis.Analyzers.Add("stopwordsBase", new CustomAnalyzer { Tokenizer = "standard", Filter = new[] { "standard", "lowercase", "stopwordsBaseFilter", "synonym" } });

                client.CreateIndex(i => i.Index(newIndexName).InitializeUsing(indexSettings));

                client.Map<TDestinationSummary>(p => p.Index(newIndexName)
                    .MapFromAttributes()
                    .Properties(prop => prop.GeoPoint(g => g.Name(n => n.Location))));
                _logger.Info("Created new vacancy search index named: {0}", newIndexName);
            }
            else
            {
                _logger.Error(string.Format("Vacancy search index already created: {0}", newIndexName));
            }
        }

        public string SwapIndex(DateTime scheduledRefreshDateTime)
        {
            _logger.Debug("Swapping vacancy search index for date: {0}", scheduledRefreshDateTime);

            var indexAlias = GetIndexAlias();
            var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime, false);
            var client = _elasticsearchClientFactory.GetElasticClient();

            _logger.Debug("Swapping vacancy search index alias to new index: {0}", newIndexName);

            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(indexAlias);
            var aliasRequest = new AliasRequest {Actions = new List<IAliasAction>()};
            
            foreach (var existingIndexOnAlias in existingIndexesOnAlias)
            {
                aliasRequest.Actions.Add(new AliasRemoveAction { Remove = new AliasRemoveOperation { Alias = indexAlias, Index = existingIndexOnAlias } });    
            }
            
            aliasRequest.Actions.Add(new AliasAddAction { Add = new AliasAddOperation { Alias = indexAlias, Index = newIndexName } });
            client.Alias(aliasRequest);

            _logger.Debug("Swapped vacancy search index alias to new index: {0}", newIndexName);

            return newIndexName;
        }

        public bool IsIndexCorrectlyCreated(DateTime scheduledRefreshDateTime)
        {
            _logger.Debug("Checking if the index is correctly created.");

            var vacanciesSource = _configurationService.Get<ServicesConfiguration>().VacanciesSource;
            if (vacanciesSource == ServicesConfiguration.Legacy)
            {
                var indexAlias = GetIndexAlias();
                var newIndexName = GetIndexNameAndDateExtension(indexAlias, scheduledRefreshDateTime, false);
                var client = _elasticsearchClientFactory.GetElasticClient();
                var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof (TDestinationSummary));

                var search = client.Search<TDestinationSummary>(s =>
                {
                    s.Index(newIndexName);
                    s.Type(documentTypeName);
                    s.Take(PageSize);
                    return s;
                });

                var result = search.Documents.Any();
                LogResult(result, newIndexName);

                return result;
            }

            if (vacanciesSource == ServicesConfiguration.Raa)
            {
                //RAA index creation talks directly to the repositories rather than a service boundary
                return true;
            }

            throw new Exception("Service implementation " + vacanciesSource + " was not recognised. Please check ServicesConfiguration section");
        }

        private void LogResult(bool result, string newIndexName)
        {
            if (result)
            {
                _logger.Debug("The index {0} is correctly created", newIndexName);
            }
            else
            {
                _logger.Error("The index {0} is not correctly created", newIndexName);
            }
        }

        private string GetIndexAlias()
        {
            return _elasticsearchClientFactory.GetIndexNameForType(typeof(TDestinationSummary));
        }

        private static string GetIndexNameAndDateExtension(string indexAlias, DateTime dateTime, bool useAlias)
        {
            return useAlias ? indexAlias : $"{indexAlias}.{dateTime.ToUniversalTime().ToString("yyyy-MM-dd-HH-mm")}";
        }
    }
}
