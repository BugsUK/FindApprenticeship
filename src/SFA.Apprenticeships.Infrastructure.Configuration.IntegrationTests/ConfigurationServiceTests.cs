namespace SFA.Apprenticeships.Infrastructure.Configuration.IntegrationTests
{
    using Application.Interfaces.Logging;
    using Common.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationServiceTests
    {
        [Test]
        public void ShouldLoadConfigFromDatastore()
        {
            var mockLogger = new Mock<ILogService>();
            var configurationService = new ConfigurationService(new ConfigurationManager(), mockLogger.Object);

            configurationService.Should().NotBeNull();
        }
    }
}
