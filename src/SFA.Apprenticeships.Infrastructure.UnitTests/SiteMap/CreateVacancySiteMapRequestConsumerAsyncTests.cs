namespace SFA.Apprenticeships.Infrastructure.UnitTests.SiteMap
{
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.SiteMap;
    using Moq;
    using NUnit.Framework;
    using Web.Common.SiteMap;

    [TestFixture]
    public class CreateVacancySiteMapRequestConsumerAsyncTests
    {
        private Mock<ILogService> _logger;
        private Mock<IConfigurationService> _configurationService;
        private Mock<IVacancySiteMapProcessor> _siteMapVacancyProcessor;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogService>();
            _configurationService = new Mock<IConfigurationService>();
            _siteMapVacancyProcessor = new Mock<IVacancySiteMapProcessor>();
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

            var consumer = new CreateVacancySiteMapRequestConsumerAsync(
                _logger.Object, _configurationService.Object, _siteMapVacancyProcessor.Object);

            var request = new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = "apprenticeships",
                TraineeshipVacancyIndexName = "vacancy"
            };

            // Act.
            var task = consumer.Consume(request);

            task.Wait();

            // Assert.
            _siteMapVacancyProcessor.Verify(mock => mock.Process(request), Times.Exactly(enabled ? 1 : 0));
        }
    }
}
