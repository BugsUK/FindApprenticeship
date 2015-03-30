namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests.Configuration
{
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Logging.IoC;
    using NUnit.Framework;
    using RabbitMq.Configuration;
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

            rabbitConfig.MessagingHost.Should().NotBeNull();
            rabbitConfig.LoggingHost.Should().NotBeNull();
        }
    }
}
