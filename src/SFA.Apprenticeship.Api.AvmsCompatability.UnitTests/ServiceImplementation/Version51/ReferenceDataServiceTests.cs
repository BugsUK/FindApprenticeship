namespace SFA.Apprenticeship.Api.AvmsCompatability.UnitTests.ServiceImplementation.Version51
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Domain.Entities.ReferenceData;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ReferenceDataService = AvmsCompatability.ServiceImplementation.Version51.ReferenceDataService;

    [TestFixture]
    public class ReferenceDataServiceTests
    {
        private ReferenceDataService _service;
        private Mock<IReferenceDataProvider> _mockReferenceDataProvider;

        [SetUp]
        public void SetUp()
        {
            _mockReferenceDataProvider = new Mock<IReferenceDataProvider>();
            _service = new ReferenceDataService(_mockReferenceDataProvider.Object);
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Arrange.
            var request = new GetApprenticeshipFrameworksRequest
            {
                MessageId = Guid.NewGuid()
            };

            // Act.
            var response = _service.GetApprenticeshipFrameworks(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.ShouldBeEquivalentTo(request.MessageId);
        }

        [Test]
        public void ShouldGetApprenticeshipFrameworks()
        {
            // Arrange.
            var request = new GetApprenticeshipFrameworksRequest();

            // Act.
            _service.GetApprenticeshipFrameworks(request);

            // Assert.
            _mockReferenceDataProvider.Verify(mock =>
                mock.GetCategories(), Times.Once());
        }

        [Test]
        public void ShouldMapApprenticeshipFrameworks()
        {
            // Arrange.
            var request = new GetApprenticeshipFrameworksRequest();

            var categories = Enumerable.Empty<Category>();

            _mockReferenceDataProvider.Setup(mock =>
                mock.GetCategories())
                .Returns(categories);

            // Act.
            var response = _service.GetApprenticeshipFrameworks(request);

            // Assert.
            response.Should().NotBeNull();
            response.ApprenticeshipFrameworks.Should().NotBeNull();
        }

        [Test]
        [Ignore]
        public void ShouldAuthenticateRequest()
        {            
        }
    }
}
