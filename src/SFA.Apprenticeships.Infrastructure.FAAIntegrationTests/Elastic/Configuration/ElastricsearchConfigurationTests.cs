namespace SFA.Apprenticeships.Infrastructure.FAAIntegrationTests.Elastic.Configuration
{
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using StructureMap;

    [TestFixture]
    public class ElastricsearchConfigurationTests
    {
        private IConfigurationService _configurationService;
        
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });

            _configurationService = container.GetInstance<IConfigurationService>();
        }


        [Test]
        public void ShouldPopulateWithValues()
        {
            var searchConfig = _configurationService.Get<SearchConfiguration>();

            searchConfig.Should().NotBeNull();
            searchConfig.HostName.Should().NotBeNull();
            searchConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            searchConfig.Timeout.Should().Be(30);

            var logstashConfig = _configurationService.Get<LogstashConfiguration>();

            logstashConfig.Should().NotBeNull();
            logstashConfig.HostName.Should().NotBeNull();
            logstashConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            logstashConfig.Timeout.Should().Be(30);
        }
    }
}
