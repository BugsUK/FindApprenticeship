namespace SFA.Apprenticeships.Application.UnitTests.Communications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Communications.Housekeeping;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class RootCommunicationHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IServiceBus> _mockServiceBus;

        private Mock<IApplicationStatusAlertCommunicationHousekeeper> _mockApplicationStatusAlertCommunicationHousekeeper;
        private Mock<IExpiringDraftApplicationAlertCommunicationHousekeeper> _mockExpiringDraftApplicationAlertCommunicationHousekeeper;
        private Mock<ISavedSearchAlertCommunicationHousekeeper> _mockSavedSearchAlertCommunicationHousekeeper;

        private RootCommunicationHousekeeper _housekeeper;
        private HousekeepingConfiguration _housekeepingConfiguration;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockServiceBus = new Mock<IServiceBus>();
            _mockServiceBus.Setup(sb => sb.PublishMessages(It.IsAny<IEnumerable<CommunicationHousekeepingRequest>>())).Returns<IEnumerable<CommunicationHousekeepingRequest>>(requests => requests.Count());

            _mockApplicationStatusAlertCommunicationHousekeeper =
                new Mock<IApplicationStatusAlertCommunicationHousekeeper>();

            _mockExpiringDraftApplicationAlertCommunicationHousekeeper =
                new Mock<IExpiringDraftApplicationAlertCommunicationHousekeeper>();

            _mockSavedSearchAlertCommunicationHousekeeper =
                new Mock<ISavedSearchAlertCommunicationHousekeeper>();

            _housekeepingConfiguration = new Fixture()
                .Build<HousekeepingConfiguration>()
                .Create();

            _mockConfigurationService
                .Setup(mock => mock
                    .Get<HousekeepingConfiguration>())
                .Returns(_housekeepingConfiguration);

            _housekeeper = new RootCommunicationHousekeeper(
                _mockLogService.Object,
                _mockConfigurationService.Object,
                _mockServiceBus.Object,
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

            // Assert.
            var expectedUniqueCount =
                statusAlertHousekeepingRequests.Count +
                expiringDraftAlertHousekeepingRequests.Count +
                savedSearchAlertHousekeepingRequests.Count - 2;

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

            // Act.
            _housekeeper.Handle(request);

            // Assert.
            _mockApplicationStatusAlertCommunicationHousekeeper.Verify(mock => mock
                .Handle(request), Times.Once);
        }

        [TestCase(0, 1, 1, 1)]
        [TestCase(1, 0, 1, 1)]
        [TestCase(1, 1, 0, 1)]
        [TestCase(1, 1, 1, 0)]
        public void ShouldNotQueueHousekeepingRequestsIfConfigurationInvalid(
            int housekeepingCycleInHours,
            int hardDeleteApplicationStatusAlertAfterCycles,
            int hardDeleteExpiringDraftApplicationAlertAfterCycles,
            int hardDeleteSavedSearchAlertAfterCycles)
        {
            // Arrange.
            _housekeepingConfiguration.HousekeepingCycleInHours = housekeepingCycleInHours;

            _housekeepingConfiguration.Communication = new CommunicationHousekeepingConfiguration
            {
                HardDeleteApplicationStatusAlertAfterCycles = hardDeleteApplicationStatusAlertAfterCycles,
                HardDeleteExpiringDraftApplicationAlertAfterCycles = hardDeleteExpiringDraftApplicationAlertAfterCycles,
                HardDeleteSavedSearchAlertAfterCycles = hardDeleteSavedSearchAlertAfterCycles
            };

            // Act.
            _housekeeper.QueueHousekeepingRequests();

            // Assert.
            _mockApplicationStatusAlertCommunicationHousekeeper.Verify(mock => mock
                .GetHousekeepingRequests(), Times.Never);

            _mockExpiringDraftApplicationAlertCommunicationHousekeeper.Verify(mock => mock
                    .GetHousekeepingRequests(), Times.Never);

            _mockSavedSearchAlertCommunicationHousekeeper.Verify(mock => mock
                .GetHousekeepingRequests(), Times.Never);

            _mockLogService.Verify(mock => mock
                .Error(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
        }
    }
}