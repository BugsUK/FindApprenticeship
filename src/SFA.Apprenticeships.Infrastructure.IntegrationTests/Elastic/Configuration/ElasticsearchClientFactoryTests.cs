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

    [TestFixture, Category("Integration")]
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

        [Test, Category("Integration")]
        public void ShouldReturnIndexNamesFromConfigurationForMappedObjectType()
        {
            _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary)).Should().EndWith("apprenticeships");
            _elasticsearchClientFactory.GetIndexNameForType(typeof(TraineeshipSummary)).Should().EndWith("traineeships");
            _elasticsearchClientFactory.GetIndexNameForType(typeof(LocationLookup)).Should().EndWith("locations");
        }

        [Test, Category("Integration")]
        public void ShouldReturnDocumentTypesFromConfigurationForMappedObjectType()
        {
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(ApprenticeshipSummary)).Should().EndWith("apprenticeship");
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(TraineeshipSummary)).Should().EndWith("traineeship");
            _elasticsearchClientFactory.GetDocumentNameForType(typeof(LocationLookup)).Should().EndWith("locationdatas");
        }
    }
}
