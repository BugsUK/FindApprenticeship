namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51
{
    using System;
    using AvService.ServiceImplementation.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyManagementServiceTests
    {
        private Mock<IVacancyUploadProvider> _mockVacancyUploadProvider;
        private VacancyManagementService _vacancyManagementService;

        [SetUp]
        public void SetUp()
        {
            _mockVacancyUploadProvider = new Mock<IVacancyUploadProvider>();
            _vacancyManagementService = new VacancyManagementService(_mockVacancyUploadProvider.Object);
        }

        [Test]
        public void ShouldUploadVacancy()
        {
            // Arrange.
            var request = new VacancyUploadRequest();
            var expectedResponse = new VacancyUploadResponse();

            _mockVacancyUploadProvider.Setup(mock =>
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
    }
}
