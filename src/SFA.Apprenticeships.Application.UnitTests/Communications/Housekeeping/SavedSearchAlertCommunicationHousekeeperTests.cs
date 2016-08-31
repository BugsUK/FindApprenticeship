namespace SFA.Apprenticeships.Application.UnitTests.Communications.Housekeeping
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Communications.Housekeeping;
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class SavedSearchAlertCommunicationHousekeeperTests
    {
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<ISavedSearchAlertRepository> _mockSavedSearchAlertRepository;
        private Mock<ICommunicationHousekeeper> _mockSuccessor;

        private HousekeepingConfiguration _housekeepingConfiguration;
        private SavedSearchAlertCommunicationHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockSavedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();

            _housekeepingConfiguration = new Fixture()
                .Build<HousekeepingConfiguration>()
                .Create();

            _mockConfigurationService
                .Setup(mock => mock
                    .Get<HousekeepingConfiguration>())
                .Returns(_housekeepingConfiguration);

            _mockSuccessor = new Mock<ICommunicationHousekeeper>();

            _housekeeper = new SavedSearchAlertCommunicationHousekeeper(
                _mockConfigurationService.Object,
                _mockSavedSearchAlertRepository.Object)
            {
                Successor = _mockSuccessor.Object
            };
        }

        [Test]
        public void ShouldGetCommunicationsForHousekeeping()
        {
            // Arrange.
            var communicationIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _mockSavedSearchAlertRepository
                .Setup(mock => mock
                    .GetAlertsCreatedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(communicationIds);

            // Act.
            var requests = _housekeeper.GetHousekeepingRequests().ToList();

            // Assert.
            requests.Select(each => each.CommunicationId).ShouldAllBeEquivalentTo(communicationIds);
            Assert.That(requests.All(each => each.CommunicationType == CommunicationTypes.SavedSearchAlert));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(30)]
        public void ShouldHonourHousekeepingConfiguration(
            int hardDeleteSavedSearchAlertAfterCycles)
        {
            // Arrange.
            var communicationIds = new[]
            {
                Guid.NewGuid()
            };

            _housekeepingConfiguration.Communication.HardDeleteSavedSearchAlertAfterCycles = hardDeleteSavedSearchAlertAfterCycles;

            var expectedHousekeepingDateTime = GetHousekeepingDateTime(_housekeepingConfiguration);
            var actualHousekeepingDateTime = new DateTime();

            _mockSavedSearchAlertRepository
                .Setup(mock => mock
                    .GetAlertsCreatedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(communicationIds)
                .Callback<DateTime>(dateTime => actualHousekeepingDateTime = dateTime);

            // Act.
            _housekeeper.GetHousekeepingRequests();

            // Assert.
            actualHousekeepingDateTime.Should().BeCloseTo(expectedHousekeepingDateTime, 500);
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void ShouldHandleSavedSearchAlertDueForHousekeeping(
            int dateCreatedOffsetHours, bool shouldHandle)
        {
            // Arrange.
            var communicationId = Guid.NewGuid();
            var dateCreated = GetHousekeepingDateTime(_housekeepingConfiguration)
                .AddHours(-dateCreatedOffsetHours);

            var savedSearchAlert = new Fixture()
                .Build<SavedSearchAlert>()
                .With(fixture => fixture.EntityId, communicationId)
                .With(fixture => fixture.DateCreated, dateCreated)
                .Create();

            _mockSavedSearchAlertRepository
                .Setup(mock => mock
                    .Get(communicationId))
                .Returns(savedSearchAlert);

            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = communicationId,
                CommunicationType = CommunicationTypes.SavedSearchAlert
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockSavedSearchAlertRepository.Verify(mock => mock
                .Delete(savedSearchAlert), Times.Exactly(shouldHandle ? 1 : 0));

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);
        }

        [Test]
        public void ShouldHandleSavedSearchAlertAlreadyDeleted()
        {
            // Arrange.
            _mockSavedSearchAlertRepository
                .Setup(mock => mock
                    .Get(It.IsAny<Guid>()))
                .Returns(default(SavedSearchAlert));

            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.SavedSearchAlert
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockSavedSearchAlertRepository.Verify(mock => mock
                .Get(It.IsAny<Guid>()), Times.Once);

            _mockSavedSearchAlertRepository.Verify(mock => mock
                .Delete(It.IsAny<SavedSearchAlert>()), Times.Never);

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);
        }

        [Test]
        public void ShouldCallSuccessor()
        {
            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.ExpiringDraftApplicationAlert
            };

            // Act.
            _housekeeper.Handle(request);

            // Assert.
            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Once);
        }

        [Test]
        public void ShouldTolerateNoSuccessor()
        {
            // Arrange.
            _housekeeper.Successor = null;

            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.ExpiringDraftApplicationAlert
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);
        }

        #region Helpers

        private static DateTime GetHousekeepingDateTime(HousekeepingConfiguration housekeepingConfiguration)
        {
            return DateTime.UtcNow.AddHours(
                -housekeepingConfiguration.HousekeepingCycleInHours *
                housekeepingConfiguration.Communication.HardDeleteSavedSearchAlertAfterCycles);
        }

        #endregion
    }
}
