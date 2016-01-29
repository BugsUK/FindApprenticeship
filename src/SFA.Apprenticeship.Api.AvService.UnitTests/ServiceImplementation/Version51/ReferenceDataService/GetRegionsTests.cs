namespace SFA.Apprenticeship.Api.AvService.UnitTests.ServiceImplementation.Version51.ReferenceDataService
{
    using System;
    using System.Collections.Generic;
    using AvService.Mediators.Version51;
    using DataContracts.Version51;
    using FluentAssertions;
    using Infrastructure.Interfaces;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using ReferenceDataService = AvService.ServiceImplementation.Version51.ReferenceDataService;

    [TestFixture]
    public class GetRegionsTests
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
        public void ShouldGetRegions()
        {
            // Arrange.
            var request = new GetRegionRequest();

            var expectedResponse = new GetRegionResponse
            {
                MessageId = Guid.NewGuid(),
                Regions = new List<RegionData>()
            };

            _mockReferenceDataServiceMediator.Setup(mock => mock
                .GetRegions(request))
                .Returns(expectedResponse);

            // Act.
            var actualResponse = _service.GetRegion(request);

            // Assert.
            actualResponse.Should().Be(expectedResponse);
        }

        [Test]
        public void ShouldThrowIfdRequestIsNull()
        {
            // Act.
            Action action = () => _service.GetRegion(default(GetRegionRequest));

            // Assert.
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ShouldLogException()
        {
            // Arrange.
            var exception = new InvalidOperationException();

            _mockReferenceDataServiceMediator.Setup(mock => mock
                .GetRegions(It.IsAny<GetRegionRequest>()))
                .Throws(exception);

            // Act.
            Action action = () => _service.GetRegion(new GetRegionRequest());

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
