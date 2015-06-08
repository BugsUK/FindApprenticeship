﻿namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using System;
    using Application.Candidates.Strategies;
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
    }
}