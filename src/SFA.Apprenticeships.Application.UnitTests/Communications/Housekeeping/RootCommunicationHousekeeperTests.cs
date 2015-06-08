namespace SFA.Apprenticeships.Application.UnitTests.Communications.Housekeeping
{
    using System;
    using System.Linq;
    using Application.Communications.Housekeeping;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class RootCommunicationHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IMessageBus> _mockMessageBus;

        private Mock<IApplicationStatusAlertCommunicationHousekeeper> _mockApplicationStatusAlertCommunicationHousekeeper;
        private Mock<IExpiringDraftApplicationAlertCommunicationHousekeeper> _mockExpiringDraftApplicationAlertCommunicationHousekeeper;
        private Mock<ISavedSearchAlertCommunicationHousekeeper> _mockSavedSearchAlertCommunicationHousekeeper;

        private RootCommunicationHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockMessageBus = new Mock<IMessageBus>();

            _mockApplicationStatusAlertCommunicationHousekeeper = new Mock<IApplicationStatusAlertCommunicationHousekeeper>();
            _mockExpiringDraftApplicationAlertCommunicationHousekeeper = new Mock<IExpiringDraftApplicationAlertCommunicationHousekeeper>();
            _mockSavedSearchAlertCommunicationHousekeeper = new Mock<ISavedSearchAlertCommunicationHousekeeper>();

            _housekeeper = new RootCommunicationHousekeeper(
                _mockLogService.Object,
                _mockMessageBus.Object,
                _mockApplicationStatusAlertCommunicationHousekeeper.Object,
                _mockExpiringDraftApplicationAlertCommunicationHousekeeper.Object,
                _mockSavedSearchAlertCommunicationHousekeeper.Object);
        }

        [Test]
        public void ShouldQueueUniqueCommunicationsForHousekeeping()
        {
            // Arrange.
            var statusAlertHousekeepingRequests = new Fixture()
                .Build<CommunicationHousekeepingRequest>()
                .CreateMany(3)
                .ToList();

            _mockApplicationStatusAlertCommunicationHousekeeper.Setup(mock => mock
                .GetHousekeepingRequests())
                .Returns(statusAlertHousekeepingRequests);

            var expiringDraftAlertHousekeepingRequests = new Fixture()
                .Build<CommunicationHousekeepingRequest>()
                .CreateMany(5)
                .ToList();

            _mockExpiringDraftApplicationAlertCommunicationHousekeeper.Setup(mock => mock
                .GetHousekeepingRequests())
                .Returns(expiringDraftAlertHousekeepingRequests);

            var savedSearchAlertHousekeepingRequests = new Fixture()
                .Build<CommunicationHousekeepingRequest>()
                .CreateMany(7)
                .ToList();

            _mockSavedSearchAlertCommunicationHousekeeper.Setup(mock => mock
                .GetHousekeepingRequests())
                .Returns(savedSearchAlertHousekeepingRequests);

            statusAlertHousekeepingRequests.First().CommunicationId =
                expiringDraftAlertHousekeepingRequests.First().CommunicationId =
                    savedSearchAlertHousekeepingRequests.First().CommunicationId;

            // Act.
            var count = _housekeeper.QueueHousekeepingRequests();
            var expectedUniqueCount =
                statusAlertHousekeepingRequests.Count +
                expiringDraftAlertHousekeepingRequests.Count +
                savedSearchAlertHousekeepingRequests.Count - 2;

            // Assert.
            count.Should().Be(expectedUniqueCount);
        }
        
        [Test]
        public void ShouldStartHousekeeperChainOfResponsibility()
        {
            // Arrange.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.Unknown
            };

            // Assert.
            _housekeeper.Handle(request);

            _mockApplicationStatusAlertCommunicationHousekeeper.Verify(mock => mock
                .Handle(request), Times.Once);
        }
    }
}
