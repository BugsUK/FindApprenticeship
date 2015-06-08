namespace SFA.Apprenticeships.Application.UnitTests.Applications.Housekeeping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Applications.Housekeeping;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class RootApplicationHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IMessageBus> _mockMessageBus;

        private Mock<IDraftApplicationForExpiredVacancyHousekeeper> _mockDraftApplicationForExpiredVacancyHousekeeper;
        private Mock<ISubmittedApplicationHousekeeper> _mockSubmittedApplicationHousekeeper;

        private RootApplicationHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockMessageBus = new Mock<IMessageBus>();

            _mockDraftApplicationForExpiredVacancyHousekeeper =
                new Mock<IDraftApplicationForExpiredVacancyHousekeeper>();

            _mockSubmittedApplicationHousekeeper =
                new Mock<ISubmittedApplicationHousekeeper>();

            _housekeeper = new RootApplicationHousekeeper(
                _mockLogService.Object,
                _mockMessageBus.Object,
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

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<ApplicationHousekeepingRequest>()))
                .Callback<ApplicationHousekeepingRequest>(queue.Add);

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

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<ApplicationHousekeepingRequest>()))
                .Callback<ApplicationHousekeepingRequest>(queue.Add);

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

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<ApplicationHousekeepingRequest>()))
                .Callback<ApplicationHousekeepingRequest>(queue.Add);

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
    }
}