namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using AvService.Providers.Version51;
    using AvService.ServiceImplementation.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ServiceContracts.Version51;

    [TestFixture]
    public class VacancyManagementServiceTests
    {
        private Mock<IVacancyUploadServiceMediator> _mockVacancyUploadServiceMediator;
        private Mock<ILogService> _mockLogService;

        private IVacancyManagement _vacancyManagementService;

        [SetUp]
        public void SetUp()
        {
            _mockVacancyUploadServiceMediator = new Mock<IVacancyUploadServiceMediator>();
            _mockLogService = new Mock<ILogService>();

            _vacancyManagementService = new VacancyManagementService(
                _mockLogService.Object,
                _mockVacancyUploadServiceMediator.Object);
        }

        [Test]
        public void ShouldUploadVacancy()
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                MessageId = Guid.NewGuid()
            };

            var expectedResponse = new VacancyUploadResponse();

            _mockVacancyUploadServiceMediator.Setup(mock =>
                mock.UploadVacancies(request))
                .Returns(expectedResponse);

            // Act.
            var actualResponse = _vacancyManagementService.UploadVacancies(request);

            // Assert.
            actualResponse.Should().Be(expectedResponse);
        }

        [Test]
        public void ShouldThrowIfVacancyUploadRequestIsNull()
        {
            // Act.
            Action action = () => _vacancyManagementService.UploadVacancies(default(VacancyUploadRequest));

            // Assert.
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        [Description("This is a temporary test and should be removed when API authentication is implemented.")]
        public void ShouldThrowIfMessageIdIsEmptyGuid()
        {
            // Arrange.
            var request = new VacancyUploadRequest
            {
                MessageId = Guid.Empty
            };

            // Act.
            Action action = () => _vacancyManagementService.UploadVacancies(request);

            // Assert.
            action.ShouldThrow<SecurityException>();
        }
    }
}
