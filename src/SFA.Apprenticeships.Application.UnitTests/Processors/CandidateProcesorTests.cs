namespace SFA.Apprenticeships.Application.UnitTests.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidates;
    using Application.Candidates.Entities;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CandidateProcesorTests
    {
        private Mock<ILogService> _mockLogger;
        private Mock<IMessageBus> _mockMessageBus;
        private Mock<IUserReadRepository> _mockUserReadRepository;
        private Mock<ICandidateReadRepository> _mockCandidateReadRepository;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();
            _mockMessageBus = new Mock<IMessageBus>();
            _mockUserReadRepository = new Mock<IUserReadRepository>();
            _mockCandidateReadRepository = new Mock<ICandidateReadRepository>();
        }

        [Test]
        public void ShouldQueueCandidatesPendingActivationOrDeletion()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object, _mockMessageBus.Object, _mockUserReadRepository.Object, _mockCandidateReadRepository.Object);

            var users = new Fixture()
                .Build<User>()
                .CreateMany(5)
                .ToList();

            var candidateIds = new List<Guid>();
            var userStatuses = new[] { UserStatuses.PendingActivation, UserStatuses.PendingDeletion };

            _mockUserReadRepository.Setup(mock =>
                mock.GetUsersWithStatus(userStatuses))
                .Returns(users);

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()))
                .Callback<CandidateHousekeeping>(message => candidateIds.Add(message.CandidateId));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()), Times.Exactly(users.Count()));

            candidateIds.Count().Should().Be(users.Count());
            candidateIds.Should().BeEquivalentTo(users.Select(each => each.EntityId));
        }

        [Test]
        public void ShouldQueueCandidatesForMobileVerificationReminders()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object, _mockMessageBus.Object, _mockUserReadRepository.Object, _mockCandidateReadRepository.Object);

            var candidates = new Fixture()
                .Build<Domain.Entities.Candidates.Candidate>()
                .CreateMany(5)
                .ToList();

            var candidateIds = new List<Guid>();

            _mockCandidateReadRepository.Setup(mock =>
                mock.GetCandidatesWithPendingMobileVerification())
                .Returns(candidates);

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()))
                .Callback<CandidateHousekeeping>(each => candidateIds.Add(each.CandidateId));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()), Times.Exactly(candidates.Count()));

            candidateIds.Count().Should().Be(candidates.Count());
            candidateIds.Should().BeEquivalentTo(candidates.Select(each => each.EntityId));
        }

        [Test]
        public void ShouldNotQueueCandidateTwice()
        {
            // Arrange.
            var processor = new CandidateProcessor(
                _mockLogger.Object, _mockMessageBus.Object, _mockUserReadRepository.Object, _mockCandidateReadRepository.Object);

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
                .Returns(users);

            _mockCandidateReadRepository.Setup(mock =>
                mock.GetCandidatesWithPendingMobileVerification())
                .Returns(candidates);

            _mockMessageBus.Setup(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()))
                .Callback<CandidateHousekeeping>(each => candidateIds.Add(each.CandidateId));

            // Act.
            processor.QueueCandidates();

            // Assert.
            _mockMessageBus.Verify(mock =>
                mock.PublishMessage(It.IsAny<CandidateHousekeeping>()), Times.Exactly(uniqueCandidateIds.Count()));

            uniqueCandidateIds.Count().Should().Be(users.Count() + candidates.Count() - 1);
            candidateIds.Count().Should().Be(uniqueCandidateIds.Count());
            candidateIds.Should().BeEquivalentTo(uniqueCandidateIds);
        }
    }
}