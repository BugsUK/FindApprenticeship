namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.IntegrationTests.Configuration
{
    using Common.Configuration;
    using FluentAssertions;
    using Infrastructure.Common.IoC;
    using IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ElastricsearchConfigurationTests
    {
        private Container smContainer = null;
        
        [SetUp]
        public void SetUp()
        {
            smContainer = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });
        }


        [Test]
        public void ShouldPopulateWithValues()
        {
            var searchConfig = smContainer.GetInstance<ElasticsearchConfiguration>(ElasticsearchConfiguration.SearchConfigurationName);

            searchConfig.Should().NotBeNull();
            searchConfig.HostName.Should().NotBeNull();
            searchConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            searchConfig.Timeout.Should().Be(30);
            
            var logstashConfig = smContainer.GetInstance<ElasticsearchConfiguration>(ElasticsearchConfiguration.LogstashConfigurationName);

            logstashConfig.Should().NotBeNull();
            logstashConfig.HostName.Should().NotBeNull();
            logstashConfig.NodeCount.Should().BeGreaterOrEqualTo(1);
            logstashConfig.Timeout.Should().Be(30);
        }
    }
}
