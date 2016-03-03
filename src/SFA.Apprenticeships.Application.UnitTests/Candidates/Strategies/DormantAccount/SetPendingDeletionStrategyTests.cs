namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using System;
    using Apprenticeships.Application.Candidates.Strategies;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SetPendingDeletionStrategyTests
    {
        [Test]
        public void UserMustBeDormant()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-365);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.Active).WithLastLogin(lastLogin).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
            savedUser.Should().BeNull();
        }

        [Test]
        public void UserMustHaveFailedToLoginAfterAtLeast330Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-364);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.Dormant).WithLastLogin(lastLogin).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
            savedUser.Should().BeNull();
        }

        [Test]
        public void Success()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-365);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.Dormant).WithLastLogin(lastLogin).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, null), Times.Never);
            savedUser.Should().NotBeNull();
            savedUser.Status.Should().Be(UserStatuses.PendingDeletion);
        }

        [Test]
        public void MustNotBeExistingUserPendingDeletion()
        {
            const string username = "user@test.com";
            var lastLogin = DateTime.UtcNow.AddDays(-365);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(username, candidateId).WithStatus(UserStatuses.Dormant).WithLastLogin(lastLogin).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            var userPendingDeletion = new UserBuilder(username, Guid.NewGuid()).WithStatus(UserStatuses.PendingDeletion).Build();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(userPendingDeletion);
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userReadRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }
    }
}