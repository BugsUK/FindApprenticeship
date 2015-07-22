namespace SFA.Apprenticeships.Infrastructure.UnitTests.SiteMap
{
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Application.Vacancies.Entities.SiteMap;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.SiteMap;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CreateVacancySiteMapRequestSubscriberTests
    {
        private Mock<ILogService> _logger;
        private Mock<IConfigurationService> _configurationService;
        private Mock<ISiteMapVacancyProcessor> _siteMapVacancyProcessor;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogService>();
            _configurationService = new Mock<IConfigurationService>();
            _siteMapVacancyProcessor = new Mock<ISiteMapVacancyProcessor>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldHonourFeatureSwitch(bool enabled)
        {
            // Arrange.
            _configurationService
                .Setup(mock => mock.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    EnableVacancySiteMap = enabled
                });

            var subscriber = new CreateVacancySiteMapRequestSubscriber(
                _logger.Object, _configurationService.Object, _siteMapVacancyProcessor.Object);

            var request = new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = "apprenticeships",
                TraineeshipVacancyIndexName = "vacancy"
            };

            // Act.
            var result = subscriber.Consume(request);

            // Assert.
            result.Should().NotBeNull();
            result.State.Should().Be(ServiceBusMessageStates.Complete);
            result.RequeueDateTimeUtc.HasValue.Should().BeFalse();

            _siteMapVacancyProcessor.Verify(mock => mock.Process(request), Times.Exactly(enabled ? 1 : 0));
        }
    }
}
