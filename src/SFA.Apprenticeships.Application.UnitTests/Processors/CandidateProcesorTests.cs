namespace SFA.Apprenticeships.Application.UnitTests.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidates;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Candidates.Entities;
    using Candidates.Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class CandidateProcesorTests
    {
        private Mock<ILogService> _mockLogger;
        private Mock<IServiceBus> _mockMessageBus;
        private Mock<IUserReadRepository> _mockUserReadRepository;
        private Mock<ICandidateReadRepository> _mockCandidateReadRepository;
        private Mock<IConfigurationService> _mockConfigurationService;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();
            _mockMessageBus = new Mock<IServiceBus>();
            _mockUserReadRepository = new Mock<IUserReadRepository>();
            _mockCandidateReadRepository = new Mock<ICandidateReadRepository>();
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockConfigurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
        }

        [Test]
        public void ShouldQueueCandidatesPendingActivationDormantOrDeletion()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object,
                _mockConfigurationService.Object,
                _mockMessageBus.Object,
                _mockUserReadRepository.Object,
                _mockCandidateReadRepository.Object);

            var users = new Fixture()
                .Build<User>()
                .CreateMany(5)
                .ToList();

            var candidateIds = new List<Guid>();
            var userStatuses = new[] { UserStatuses.PendingActivation, UserStatuses.PendingDeletion };

            _mockUserReadRepository.Setup(mock =>
                mock.GetUsersWithStatus(userStatuses))
                .Returns(users.Select(u => u.EntityId));

            _mockMessageBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()))
                .Callback<IEnumerable<CandidateHousekeeping>>(messages => candidateIds.AddRange(messages.Select(cid => cid.CandidateId)));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()), Times.Once);

            candidateIds.Count().Should().Be(users.Count());
            candidateIds.Should().BeEquivalentTo(users.Select(each => each.EntityId));
        }

        [Test]
        public void ShouldQueueCandidatesForMobileVerificationReminders()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object,
                _mockConfigurationService.Object,
                _mockMessageBus.Object,
                _mockUserReadRepository.Object,
                _mockCandidateReadRepository.Object);

            var candidates = new Fixture()
                .Build<Domain.Entities.Candidates.Candidate>()
                .CreateMany(5)
                .ToList();

            var candidateIds = new List<Guid>();

            _mockCandidateReadRepository.Setup(mock =>
                mock.GetCandidatesWithPendingMobileVerification())
                .Returns(candidates.Select(c => c.EntityId));

            _mockMessageBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()))
                .Callback<IEnumerable<CandidateHousekeeping>>(messages => candidateIds.AddRange(messages.Select(cid => cid.CandidateId)));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()), Times.Once);

            candidateIds.Count().Should().Be(candidates.Count());
            candidateIds.Should().BeEquivalentTo(candidates.Select(each => each.EntityId));
        }

        [Test]
        public void ShouldNotQueueCandidateTwice()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object,
                _mockConfigurationService.Object,
                _mockMessageBus.Object,
                _mockUserReadRepository.Object,
                _mockCandidateReadRepository.Object);

            var users = new Fixture()
                .Build<User>()
                .CreateMany(2)
                .ToList();

            var candidates = new Fixture()
                .Build<Domain.Entities.Candidates.Candidate>()
                .CreateMany(3)
                .ToList();

            users.First().EntityId = candidates.Last().EntityId = Guid.NewGuid();

            var candidateIds = new List<Guid>();
            var userStatuses = new[] { UserStatuses.PendingActivation, UserStatuses.PendingDeletion };

            var uniqueCandidateIds = candidates
                .Select(each => each.EntityId)
                .Union(users.Select(each => each.EntityId))
                .ToList();

            _mockUserReadRepository.Setup(mock =>
                mock.GetUsersWithStatus(userStatuses))
                .Returns(users.Select(u => u.EntityId));

            _mockCandidateReadRepository.Setup(mock =>
                mock.GetCandidatesWithPendingMobileVerification())
                .Returns(candidates.Select(c => c.EntityId));

            _mockMessageBus.Setup(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()))
                .Callback<IEnumerable<CandidateHousekeeping>>(messages => candidateIds.AddRange(messages.Select(cid => cid.CandidateId)));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessages(It.IsAny<IEnumerable<CandidateHousekeeping>>()), Times.Once);

            uniqueCandidateIds.Count().Should().Be(users.Count() + candidates.Count() - 1);
            candidateIds.Count().Should().Be(uniqueCandidateIds.Count());
            candidateIds.Should().BeEquivalentTo(uniqueCandidateIds);
        }
    }
}