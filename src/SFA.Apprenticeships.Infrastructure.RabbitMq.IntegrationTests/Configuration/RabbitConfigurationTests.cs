namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests.Configuration
{
    using Application.Interfaces.Logging;
    using Common.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using RabbitMq.Configuration;

    [TestFixture]
    public class RabbitConfigurationTests
    {
        [Test]
        public void LoadConfigurationFromDatabase()
        {
            var mockLogger = new Mock<ILogService>();
            var configurationService = new ConfigurationService(new ConfigurationManager(), mockLogger.Object);

            var rabbitConfig = configurationService.Get<RabbitConfiguration>();

            rabbitConfig.MessagingHost.Should().NotBeNull();
            rabbitConfig.LoggingHost.Should().NotBeNull();
        }
    }
}
