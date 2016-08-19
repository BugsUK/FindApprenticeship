namespace SFA.Apprenticeships.Application.UnitTests.Applications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Applications.Housekeeping;
    using Apprenticeships.Application.Candidates.Configuration;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class RootApplicationHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IServiceBus> _mockServiceBus;

        private Mock<IDraftApplicationForExpiredVacancyHousekeeper> _mockDraftApplicationForExpiredVacancyHousekeeper;
        private Mock<ISubmittedApplicationHousekeeper> _mockSubmittedApplicationHousekeeper;

        private RootApplicationHousekeeper _housekeeper;
        private HousekeepingConfiguration _housekeepingConfiguration;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockServiceBus = new Mock<IServiceBus>();

            _mockDraftApplicationForExpiredVacancyHousekeeper =
                new Mock<IDraftApplicationForExpiredVacancyHousekeeper>();

            _mockSubmittedApplicationHousekeeper =
                new Mock<ISubmittedApplicationHousekeeper>();

            _housekeepingConfiguration = new Fixture()
                .Build<HousekeepingConfiguration>()
                .Create();

            _mockConfigurationService
                .Setup(mock => mock
                    .Get<HousekeepingConfiguration>())
                .Returns(_housekeepingConfiguration);

            _housekeeper = new RootApplicationHousekeeper(
                _mockLogService.Object,
                _mockConfigurationService.Object,
                _mockServiceBus.Object,
                _mockDraftApplicationForExpiredVacancyHousekeeper.Object,
                _mockSubmittedApplicationHousekeeper.Object);
        }

        [Test]
        public void ShouldQueueDraftApplicationForExpiredVacanciesForHousekeeping()
        {
            // Arrange.
            var requests = new Fixture()
                .Build<ApplicationHousekeepingRequest>()
                .CreateMany(3)
                .ToList();

            var queue = new List<ApplicationHousekeepingRequest>();

            _mockServiceBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<ApplicationHousekeepingRequest>>()))
                .Callback<IEnumerable<ApplicationHousekeepingRequest>>(queue.AddRange).Returns<IEnumerable<ApplicationHousekeepingRequest>>(messages => messages.Count());

            _mockDraftApplicationForExpiredVacancyHousekeeper
                .Setup(mock => mock
                    .GetHousekeepingRequests())
                .Returns(requests);

            // Act.
            var count = _housekeeper.QueueHousekeepingRequests();

            // Assert.
            _mockDraftApplicationForExpiredVacancyHousekeeper.Verify(mock =>
                mock.GetHousekeepingRequests(), Times.Once);

            count.Should().Be(requests.Count);
            queue.ShouldAllBeEquivalentTo(requests);
        }

        [Test]
        public void ShouldQueueSubmittedApplicationsForHousekeeping()
        {
            // Arrange.
            var requests = new Fixture()
                .Build<ApplicationHousekeepingRequest>()
                .CreateMany(3)
                .ToList();

            var queue = new List<ApplicationHousekeepingRequest>();

            _mockServiceBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<ApplicationHousekeepingRequest>>()))
                .Callback<IEnumerable<ApplicationHousekeepingRequest>>(queue.AddRange).Returns<IEnumerable<ApplicationHousekeepingRequest>>(messages => messages.Count());

            _mockSubmittedApplicationHousekeeper
                .Setup(mock => mock
                    .GetHousekeepingRequests())
                .Returns(requests);

            // Act.
            var count = _housekeeper.QueueHousekeepingRequests();

            // Assert.
            count.Should().Be(requests.Count);
            queue.ShouldAllBeEquivalentTo(requests);
        }

        [Test]
        public void ShouldNotQueueDuplicateApplicationsForHousekeeping()
        {
            // Arrange.
            var draftApplicationHousekeepingRequests = new Fixture()
                .Build<ApplicationHousekeepingRequest>()
                .CreateMany(3)
                .ToList();

            var submittedApplicationHousekeepingRequests = new Fixture()
                .Build<ApplicationHousekeepingRequest>()
                .CreateMany(2)
                .ToList();

            draftApplicationHousekeepingRequests.First().ApplicationId = submittedApplicationHousekeepingRequests.Last().ApplicationId;

            var queue = new List<ApplicationHousekeepingRequest>();

            _mockServiceBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<ApplicationHousekeepingRequest>>()))
                .Callback<IEnumerable<ApplicationHousekeepingRequest>>(queue.AddRange).Returns<IEnumerable<ApplicationHousekeepingRequest>>(messages => messages.Count());

            _mockDraftApplicationForExpiredVacancyHousekeeper
                .Setup(mock => mock
                    .GetHousekeepingRequests())
                .Returns(draftApplicationHousekeepingRequests);

            _mockSubmittedApplicationHousekeeper
                .Setup(mock => mock
                    .GetHousekeepingRequests())
                .Returns(submittedApplicationHousekeepingRequests);

            // Act.
            var count = _housekeeper.QueueHousekeepingRequests();
            var expectedCount = draftApplicationHousekeepingRequests.Count + submittedApplicationHousekeepingRequests.Count - 1;

            // Assert.
            _mockDraftApplicationForExpiredVacancyHousekeeper.Verify(mock =>
                mock.GetHousekeepingRequests(), Times.Once);

            _mockSubmittedApplicationHousekeeper.Verify(mock =>
                mock.GetHousekeepingRequests(), Times.Once);

            count.Should().Be(expectedCount);
            queue.ShouldAllBeEquivalentTo(draftApplicationHousekeepingRequests.Union(submittedApplicationHousekeepingRequests));
        }

        [Test]
        public void ShouldStartHousekeeperChainOfResponsibility()
        {
            // Arrange.
            var request = new ApplicationHousekeepingRequest
            {
                ApplicationId = Guid.NewGuid(),
                VacancyType = VacancyType.Unknown
            };

            // Assert.
            _housekeeper.Handle(request);

            _mockDraftApplicationForExpiredVacancyHousekeeper.Verify(mock => mock
                .Handle(request), Times.Once);
        }

        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        public void ShouldNotQueueHousekeepingRequestsIfConfigurationInvalid(
            int housekeepingCycleInHours,
            int hardDeleteDraftApplicationForExpiredVacancyAfterCycles,
            int hardDeleteSubmittedApplicationAfterCycles)
        {
            // Arrange.
            _housekeepingConfiguration.HousekeepingCycleInHours = housekeepingCycleInHours;

            _housekeepingConfiguration.Application = new ApplicationHousekeepingConfiguration
            {
                HardDeleteDraftApplicationForExpiredVacancyAfterCycles = hardDeleteDraftApplicationForExpiredVacancyAfterCycles,
                HardDeleteSubmittedApplicationAfterCycles = hardDeleteSubmittedApplicationAfterCycles
            };

            // Act.
            _housekeeper.QueueHousekeepingRequests();

            // Assert.
            _mockDraftApplicationForExpiredVacancyHousekeeper.Verify(mock => mock
                .GetHousekeepingRequests(), Times.Never);

            _mockSubmittedApplicationHousekeeper.Verify(mock => mock
                .GetHousekeepingRequests(), Times.Never);

            _mockLogService.Verify(mock => mock
                .Error(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
        }
    }
}