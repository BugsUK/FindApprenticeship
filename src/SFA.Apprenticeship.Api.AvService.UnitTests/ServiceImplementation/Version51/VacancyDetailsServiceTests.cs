namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using SFA.Infrastructure.Interfaces;
    using AvService.Providers.Version51;
    using AvService.ServiceImplementation.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ServiceContracts.Version51;

    [TestFixture]
    public class VacancyDetailsServiceTests
    {
        private IVacancyDetails _vacancyDetailsService;
        private Mock<ILogService> _mockLogService;
        private Mock<IVacancyDetailsProvider> _mockVacancyDetailsProvider;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockVacancyDetailsProvider = new Mock<IVacancyDetailsProvider>();

            _vacancyDetailsService = new VacancyDetailsService(
                _mockLogService.Object, _mockVacancyDetailsProvider.Object);
        }

        [Test]
        public void ShouldGetVacancyDetails()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.NewGuid()
            };

            var expectedResponse = new VacancyDetailsResponse();

            _mockVacancyDetailsProvider.Setup(mock =>
                mock.Get(request))
                .Returns(expectedResponse);

            // Act.
            var actualResponse = _vacancyDetailsService.Get(request);

            // Assert.
            actualResponse.Should().Be(expectedResponse);
        }

        [Test]
        public void ShouldThrowIfVacancyDetailsRequestIsNull()
        {
            // Act.
            Action action = () => _vacancyDetailsService.Get(default(VacancyDetailsRequest));

            // Assert.
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        [Description("This is a temporary test and should be removed when API authentication is implemented.")]
        public void ShouldThrowIfMessageIdIsEmptyGuid()
        {
            // Arrange.
            var request = new VacancyDetailsRequest
            {
                MessageId = Guid.Empty
            };

            // Act.
            Action action = () => _vacancyDetailsService.Get(request);

            // Assert.
            action.ShouldThrow<SecurityException>();
        }
    }
}
