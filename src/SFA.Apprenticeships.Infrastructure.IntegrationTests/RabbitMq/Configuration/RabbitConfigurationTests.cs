namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.RabbitMq.Configuration
{
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Logging.Configuration;
    using Logging.IoC;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap;

    [TestFixture]
    public class RabbitConfigurationTests
    {
        [Test]
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
