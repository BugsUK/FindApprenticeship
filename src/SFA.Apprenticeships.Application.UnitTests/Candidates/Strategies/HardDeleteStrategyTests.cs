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
        public void UsersPendingDeletionAreNotImmediatelyDeleted()
        {
            var dateUpdated = DateTime.UtcNow;
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingDeletion).WithDateUpdated(dateUpdated).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }

        [TestCase(0, false)]
        [TestCase(13, false)]
        [TestCase(14, true)]
        [TestCase(15, true)]
        public void UsersPendingDeletionAreHardDeletedAfter14Days(int days, bool shouldBeHardDeleted)
        {
            var dateUpdated = DateTime.UtcNow.AddDays(-days);
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingDeletion).WithDateUpdated(dateUpdated).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            if (shouldBeHardDeleted)
            {
                //Strategy handled the request
                successor.Verify(s => s.Handle(user, null), Times.Never);
            }
            else
            {
                //Strategy did not handle the request
                successor.Verify(s => s.Handle(user, candidate), Times.Once);
            }
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