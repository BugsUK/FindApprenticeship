﻿namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using System;
    using Application.Vacancies.Entities;
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.IoC;
    using FluentAssertions;
    using IoC;
    using Nest;
    using NUnit.Framework;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Logging.IoC;
    using StructureMap;
    using VacancyLocationType = Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLocationType;

    [TestFixture]
    public class IndexingInitialisationTests
    {
        private string _vacancyIndexAlias;
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private ElasticClient _elasticClient;
        private SearchConfiguration _elasticsearchConfiguration = null;
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
            });

            _elasticsearchConfiguration = _container.GetInstance<IConfigurationService>().Get<SearchConfiguration>();
            var settings = new ConnectionSettings(new Uri(_elasticsearchConfiguration.HostName));
            _elasticClient = new ElasticClient(settings);

            foreach (var elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                if (elasticsearchIndexConfiguration.Name.EndsWith("_integration_test"))
                {
                    _elasticClient.DeleteIndex(i => i.Index(elasticsearchIndexConfiguration.Name));
                }
            }

            _elasticClient = new ElasticClient(settings);

            _elasticsearchClientFactory = _container.GetInstance<IElasticsearchClientFactory>();

            _vacancyIndexAlias = _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary));
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var elasticsearchIndexConfiguration in _elasticsearchConfiguration.Indexes)
            {
                if (elasticsearchIndexConfiguration.Name.EndsWith("_integration_test"))
                {
                    var configuration = elasticsearchIndexConfiguration;
                    _elasticClient.DeleteIndex(i => i.Index(configuration.Name));
                }
            }
        }

        [Test, Category("Integration")]
        public void ShouldCreateScheduledIndexAndMapping()
        {
            var scheduledDate = new DateTime(2000, 1, 1);
            var indexName = string.Format("{0}.{1}", _vacancyIndexAlias, scheduledDate.ToString("yyyy-MM-dd-HH"));

            var vis = _container.GetInstance<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();

            DeleteIndexIfExists(indexName);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
            vis.CreateScheduledIndex(scheduledDate);
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeTrue();

            var mapping = _elasticClient.GetMapping<ApprenticeshipSummary>(i => i.Index(indexName));
            mapping.Should().NotBeNull();

            _elasticClient.DeleteIndex(i => i.Index(indexName));
            _elasticClient.IndexExists(i => i.Index(indexName)).Exists.Should().BeFalse();
        }

        private void DeleteIndexIfExists(string indexName)
        {
            var exists = _elasticClient.IndexExists(i => i.Index(indexName));
            if (exists.Exists)
            {
                _elasticClient.DeleteIndex(i => i.Index(indexName));
            }
        }
    }
}
