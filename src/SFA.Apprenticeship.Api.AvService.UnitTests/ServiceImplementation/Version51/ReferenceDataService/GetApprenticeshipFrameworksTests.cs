namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51.ReferenceDataService
{
    using System;
    using System.Linq;
    using AvService.Mediators.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using Infrastructure.Interfaces;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ReferenceDataService = AvService.ServiceImplementation.Version51.ReferenceDataService;

    // TODO: US868: API: reinstate / implement tests here. See GetErrorCodesTests for examples.

    [TestFixture]
    public class GetApprenticeshipFrameworksTests
    {
        private Mock<IReferenceDataServiceMediator> _mockReferenceDataServiceMediator;
        private Mock<ILogService> _mockLogService;

        private ReferenceDataService _service;

        [SetUp]
        public void SetUp()
        {

            _mockLogService = new Mock<ILogService>();
            _mockReferenceDataServiceMediator = new Mock<IReferenceDataServiceMediator>();

            _service = new ReferenceDataService(
                _mockLogService.Object,
                _mockReferenceDataServiceMediator.Object);
        }

        [Test]
        [Ignore]
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
        [Ignore]
        public void ShouldGetApprenticeshipFrameworks()
        {
            // Arrange.
            var request = new GetApprenticeshipFrameworksRequest();

            // Act.
            _service.GetApprenticeshipFrameworks(request);

            // Assert.
            _mockReferenceDataServiceMediator.Verify(mock =>
                mock.GetApprenticeshipFrameworks(), Times.Once());
        }

        [Test]
        [Ignore]
        public void ShouldMapApprenticeshipFrameworks()
        {
            // Arrange.
            var request = new GetApprenticeshipFrameworksRequest();

            var frameworks = Enumerable.Empty<ApprenticeshipFrameworkAndOccupationData>().ToList();

            _mockReferenceDataServiceMediator.Setup(mock =>
                mock.GetApprenticeshipFrameworks())
                .Returns(frameworks);

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
