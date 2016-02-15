namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.RabbitMq.Configuration
{
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Logging.Configuration;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture, Category("Integration")]
    public class RabbitConfigurationTests
    {
        [Test, Category("Integration")]
        public void LoadConfigurationFromDatabase()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var rabbitConfig = configurationService.Get<RabbitConfiguration>();

            rabbitConfig.LoggingHost.Should().NotBeNull();
        }
    }
}
