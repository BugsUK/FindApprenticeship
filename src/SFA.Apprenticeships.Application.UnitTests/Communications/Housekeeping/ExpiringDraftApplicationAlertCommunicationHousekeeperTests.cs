namespace SFA.Apprenticeships.Application.UnitTests.Communications.Housekeeping
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Communications.Housekeeping;
    using Domain.Entities.Communication;
    using Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ExpiringDraftApplicationAlertCommunicationHousekeeperTests
    {
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IExpiringApprenticeshipApplicationDraftRepository> _mockExpiringApprenticeshipApplicationDraftRepository;
        private Mock<ICommunicationHousekeeper> _mockSuccessor;

        private HousekeepingConfiguration _housekeepingConfiguration;
        private ExpiringDraftApplicationAlertCommunicationHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockExpiringApprenticeshipApplicationDraftRepository = new Mock<IExpiringApprenticeshipApplicationDraftRepository>();

            _housekeepingConfiguration = new Fixture()
                .Build<HousekeepingConfiguration>()
                .Create();

            _mockConfigurationService
                .Setup(mock => mock
                    .Get<HousekeepingConfiguration>())
                .Returns(_housekeepingConfiguration);

            _mockSuccessor = new Mock<ICommunicationHousekeeper>();

            _housekeeper = new ExpiringDraftApplicationAlertCommunicationHousekeeper(
                _mockConfigurationService.Object,
                _mockExpiringApprenticeshipApplicationDraftRepository.Object)
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

            _mockExpiringApprenticeshipApplicationDraftRepository
                .Setup(mock => mock
                    .GetAlertsCreatedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(communicationIds);

            // Act.
            var requests = _housekeeper.GetHousekeepingRequests().ToList();

            // Assert.
            requests.Select(each => each.CommunicationId).ShouldAllBeEquivalentTo(communicationIds);
            Assert.That(requests.All(each => each.CommunicationType == CommunicationTypes.ExpiringDraftApplicationAlert));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(30)]
        public void ShouldHonourHousekeepingConfiguration(
            int hardDeleteExpiringDraftApplicationAlertAfterCycles)
        {
            // Arrange.
            var communicationIds = new[]
            {
                Guid.NewGuid()
            };

            _housekeepingConfiguration.Communication.HardDeleteExpiringDraftApplicationAlertAfterCycles = hardDeleteExpiringDraftApplicationAlertAfterCycles;

            var expectedHousekeepingDateTime = GetHousekeepingDateTime(_housekeepingConfiguration);
            var actualHousekeepingDateTime = new DateTime();

            _mockExpiringApprenticeshipApplicationDraftRepository
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

            var expiringApprenticeshipApplicationDraft = new Fixture()
                .Build<ExpiringApprenticeshipApplicationDraft>()
                .With(fixture => fixture.EntityId, communicationId)
                .With(fixture => fixture.DateCreated, dateCreated)
                .Create();

            _mockExpiringApprenticeshipApplicationDraftRepository
                .Setup(mock => mock
                    .Get(communicationId))
                .Returns(expiringApprenticeshipApplicationDraft);

            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = communicationId,
                CommunicationType = CommunicationTypes.ExpiringDraftApplicationAlert
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockExpiringApprenticeshipApplicationDraftRepository.Verify(mock => mock
                .Delete(expiringApprenticeshipApplicationDraft), Times.Exactly(shouldHandle ? 1 : 0));

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);
        }

        [Test]
        public void ShouldHandleSavedSearchAlertAlreadyDeleted()
        {
            // Arrange.
            _mockExpiringApprenticeshipApplicationDraftRepository
                .Setup(mock => mock
                    .Get(It.IsAny<Guid>()))
                .Returns(default(ExpiringApprenticeshipApplicationDraft));

            // Act.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.ExpiringDraftApplicationAlert
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockExpiringApprenticeshipApplicationDraftRepository.Verify(mock => mock
                .Get(It.IsAny<Guid>()), Times.Once);

            _mockExpiringApprenticeshipApplicationDraftRepository.Verify(mock => mock
                .Delete(It.IsAny<ExpiringApprenticeshipApplicationDraft>()), Times.Never);

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);
        }

        [Test]
        public void ShouldCallSuccessor()
        {
            // Arrange.
            var request = new CommunicationHousekeepingRequest
            {
                CommunicationId = Guid.NewGuid(),
                CommunicationType = CommunicationTypes.ApplicationStatusAlert
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
                CommunicationType = CommunicationTypes.ApplicationStatusAlert
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
                housekeepingConfiguration.Communication.HardDeleteExpiringDraftApplicationAlertAfterCycles);
        }

        #endregion
    }
}
