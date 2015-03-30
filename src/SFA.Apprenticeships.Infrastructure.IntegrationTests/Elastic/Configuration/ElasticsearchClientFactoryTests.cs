namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Elastic.Configuration
{
    using FluentAssertions;
    using Common.IoC;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.Entities;
    using Infrastructure.Elastic.Common.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ElasticsearchClientFactoryTests
    {
        private IElasticsearchClientFactory _elasticsearchClientFactory;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });

            _elasticsearchClientFactory = container.GetInstance<IElasticsearchClientFactory>();
        }

        [Test]
        public void ShouldReturnIndexNamesFromConfigurationForMappedObjectType()
        {
            _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary)).Should().Be("apprenticeships");
            _elasticsearchClientFactory.GetIndexNameForType(typeof(TraineeshipSummary)).Should().Be("traineeships");
            _elasticsearchClientFactory.GetIndexNameForType(typeof(LocationLookup)).Should().Be("locations");
            _elasticsearchClientFactory.GetIndexNameForType(typeof(Address)).Should().Be("addresses");
        }

        [Test]
        public void ShouldReturnDocumentTypesFromConfigurationForMappedObjectType()
        {
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(ApprenticeshipSummary)).Should().Be("apprenticeship");
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(TraineeshipSummary)).Should().Be("traineeship");
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(LocationLookup)).Should().Be("locationdatas");
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(Address)).Should().Be("address");
        }
    }
}
