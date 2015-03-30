namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Elastic.Configuration
{
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Common.IoC;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ElastricsearchConfigurationTests
    {
        private IConfigurationService configurationService = null;
        
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });

            configurationService = container.GetInstance<IConfigurationService>();
        }


        [Test]
        public void ShouldPopulateWithValues()
        {
            var searchConfig = configurationService.Get<SearchConfiguration>();

            searchConfig.Should().NotBeNull();
            searchConfig.HostName.Should().NotBeNull();
            searchConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            searchConfig.Timeout.Should().Be(30);

            var logstashConfig = configurationService.Get<LogstashConfiguration>();

            logstashConfig.Should().NotBeNull();
            logstashConfig.HostName.Should().NotBeNull();
            logstashConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            logstashConfig.Timeout.Should().Be(30);
        }
    }
}
