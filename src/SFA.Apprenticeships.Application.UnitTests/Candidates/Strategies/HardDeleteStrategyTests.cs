using System;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Application.Candidates.Strategies;
using SFA.Apprenticeships.Domain.Entities.UnitTests.Builder;
using SFA.Apprenticeships.Domain.Entities.Users;

namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    [TestFixture]
    public class HardDeleteStrategyTests
    {
        [Test]
        public void UsersPendingDeletionAreImmediatelyDeleted()
        {
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingDeletion).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, null), Times.Never);
        }

        [Test]
        public void OnlyConsiderUsersPendingDeletion()
        {
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingActivation).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }
    }
}