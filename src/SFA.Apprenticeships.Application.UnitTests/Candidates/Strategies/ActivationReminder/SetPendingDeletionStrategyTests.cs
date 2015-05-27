namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.ActivationReminder
{
    using System;
    using Application.Candidates.Strategies;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SetPendingDeletionStrategyTests
    {
        [Test]
        public void UserMustBePendingActivation()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-31);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(true).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }

        [Test]
        public void UserMustHaveFailedToActivateAfterAtLeast31Days()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-30);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }

        [Test]
        public void SetUserPendingDeletionIfNoCandidateRecord()
        {
            var dateCreated = DateTime.UtcNow;

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(true).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, null);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, null), Times.Never);
        }

        [Test]
        public void SetPendingDeletionEndsChainOfResponsibility()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-31);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SetPendingDeletionStrategyBuilder().With(userWriteRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, candidate), Times.Never);
        }
    }
}