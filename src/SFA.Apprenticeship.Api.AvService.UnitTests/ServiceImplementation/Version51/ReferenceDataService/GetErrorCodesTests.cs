namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51.ReferenceDataService
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Interfaces.Logging;
    using AvService.Mediators.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ReferenceDataService = AvService.ServiceImplementation.Version51.ReferenceDataService;

    // TODO: US868: API: reinstate / implement tests here.

    [TestFixture]
    public class GetErrorCodesTests
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
        public void ShouldGetErrorCodes()
        {
            // Arrange.
            var request = new GetErrorCodesRequest();

            var expectedResponse = new GetErrorCodesResponse
            {
                MessageId = Guid.NewGuid(),
                ErrorCodes = new List<ErrorCodesData>()
            };

            _mockReferenceDataServiceMediator.Setup(mock => mock
                .GetErrorCodes(request))
                .Returns(expectedResponse);

            // Act.
            var actualResponse = _service.GetErrorCodes(request);

            // Assert.
            actualResponse.Should().Be(expectedResponse);
        }

        [Test]
        public void ShouldThrowIfdRequestIsNull()
        {
            // Act.
            Action action = () => _service.GetErrorCodes(default(GetErrorCodesRequest));

            // Assert.
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ShouldLogException()
        {
            // Arrange.
            var exception = new InvalidOperationException();

            _mockReferenceDataServiceMediator.Setup(mock => mock
                .GetErrorCodes(It.IsAny<GetErrorCodesRequest>()))
                .Throws(exception);

            // Act.
            Action action = () => _service.GetErrorCodes(new GetErrorCodesRequest());

            // Assert.
            action.ShouldThrowExactly<InvalidOperationException>();

            _mockLogService.Verify(mock =>
                mock.Error(exception, It.IsAny<object>()), Times.Once());
        }

        [Test]
        [Ignore]
        public void ShouldAuthenticateRequest()
        {
            Assert.Fail();
        }
    }
}
