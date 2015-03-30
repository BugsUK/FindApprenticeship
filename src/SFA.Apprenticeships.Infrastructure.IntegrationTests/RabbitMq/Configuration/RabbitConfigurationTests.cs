namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.RabbitMq.Configuration
{
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Infrastructure.RabbitMq.Configuration;
    using Logging.IoC;
    using NUnit.Framework;
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
