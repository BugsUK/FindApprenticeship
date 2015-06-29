namespace SFA.Apprenticeships.Service.Vacancy
{
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Nest;
    using StructureMap;
    using Types;
    using ApprenticeshipSummary = Infrastructure.Elastic.Common.Entities.ApprenticeshipSummary;

    public class SearchProvider
    {
        private readonly ElasticClient _client;
        private readonly string _indexName;
        private readonly string _documentTypeName;

        public SearchProvider()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<LoggingRegistry>();
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
            });

            var elasticsearchClientFactory = container.GetInstance<IElasticsearchClientFactory>();

            _client = elasticsearchClientFactory.GetElasticClient();
            _indexName = elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary));
            _documentTypeName = elasticsearchClientFactory.GetDocumentNameForType(typeof(ApprenticeshipSummary));
        }

        public SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> Search(Types.SearchRequest request)
        {
            var searchRequestExtended = new SearchRequestExtended(request);

            var search = _client.Search<ApprenticeshipSearchResponse>(s =>
            {
                s.Index(_indexName);
                s.Type(_documentTypeName);
                s.Take(1000);

                s.Query(q =>
                {
                    QueryContainer query = null;

                    if (searchRequestExtended.SearchJobTitleField)
                    {
                        if (searchRequestExtended.UseJobTitleTerms && !string.IsNullOrWhiteSpace(searchRequestExtended.JobTitleTerms))
                        {
                            var queryClause = q.Match(m =>
                            {
                                m.OnField(f => f.Title).Query(searchRequestExtended.JobTitleTerms);
                                BuildFieldQuery(m, searchRequestExtended.JobTitleFactors);
                            });

                            query = BuildContainer(null, queryClause);
                        }
                        else
                        {
                            var queryClause = q.Match(m =>
                            {
                                m.OnField(f => f.Title).Query(searchRequestExtended.KeywordTerms);
                                BuildFieldQuery(m, searchRequestExtended.JobTitleFactors);
                            });

                            query = BuildContainer(null, queryClause);
                        }
                    }

                    if (searchRequestExtended.SearchDescriptionField && !string.IsNullOrWhiteSpace(searchRequestExtended.KeywordTerms))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.Description).Query(searchRequestExtended.KeywordTerms);
                            BuildFieldQuery(m, searchRequestExtended.DescriptionFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    if (searchRequestExtended.SearchEmployerNameField && !string.IsNullOrWhiteSpace(searchRequestExtended.KeywordTerms))
                    {
                        var queryClause = q.Match(m =>
                        {
                            m.OnField(f => f.EmployerName).Query(searchRequestExtended.KeywordTerms);
                            BuildFieldQuery(m, searchRequestExtended.EmployerFactors);
                        });
                        query = BuildContainer(query, queryClause);
                    }

                    return query;
                });

                return s;
            });

            var results = search.Documents.ToList();
            results.ForEach(r => r.Score = search.HitsMetaData.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture)).Score);
            var searchResults = new SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(
                search.Total, results, null, null);

            return searchResults;
        }

        private QueryContainer BuildContainer(QueryContainer queryContainer, QueryContainer queryClause)
        {
            if (queryContainer == null)
            {
                queryContainer = queryClause;
            }
            else
            {
                queryContainer |= queryClause;
            }

            return queryContainer;
        }

        private static void BuildFieldQuery(MatchQueryDescriptor<ApprenticeshipSearchResponse> queryDescriptor, KeywordFactors searchFactors)
        {
            if (searchFactors.Boost.HasValue)
            {
                queryDescriptor.Boost(searchFactors.Boost.Value);
            }

            if (searchFactors.Fuzziness.HasValue)
            {
                queryDescriptor.Fuzziness(searchFactors.Fuzziness.Value);
            }

            if (searchFactors.FuzzinessPrefix.HasValue)
            {
                queryDescriptor.PrefixLength(searchFactors.FuzzinessPrefix.Value);
            }

            if (searchFactors.MatchAllKeywords)
            {
                queryDescriptor.Operator(Operator.And);
            }

            if (!string.IsNullOrWhiteSpace(searchFactors.MinimumMatch))
            {
                queryDescriptor.MinimumShouldMatch(searchFactors.MinimumMatch);
            }

            if (searchFactors.PhraseProximity.HasValue)
            {
                queryDescriptor.Slop(searchFactors.PhraseProximity.Value);
            }   
        }
    }
}
