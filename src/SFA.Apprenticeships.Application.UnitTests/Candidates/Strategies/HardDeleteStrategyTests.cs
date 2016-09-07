namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using Apprenticeships.Application.Candidates.Strategies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            var dateUpdated = DateTime.UtcNow.AddDays(-14);
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingActivation).WithDateUpdated(dateUpdated).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder().With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy did not handle the request
            successor.Verify(s => s.Handle(user, candidate), Times.Once);
        }

        [Test]
        public void DeleteIfUserIsNull()
        {
            var candidateId = Guid.NewGuid();
            User user = null;
            var candidate = new CandidateBuilder(candidateId).Build();

            var successor = new Mock<IHousekeepingStrategy>();
            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var authenticationRepository = new Mock<IAuthenticationRepository>();
            var strategy = new HardDeleteStrategyBuilder().With(userWriteRepository).With(candidateWriteRepository).With(authenticationRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, candidate), Times.Never);

            //Entities were deleted
            candidateWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
            authenticationRepository.Verify(r => r.Delete(candidateId), Times.Once);
            userWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
        }

        [Test]
        public void DeleteIfCandidateIsNull()
        {
            var dateUpdated = DateTime.UtcNow.AddDays(-14);
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingActivation).WithDateUpdated(dateUpdated).Build();
            Candidate candidate = null;

            var successor = new Mock<IHousekeepingStrategy>();
            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var authenticationRepository = new Mock<IAuthenticationRepository>();
            var strategy = new HardDeleteStrategyBuilder().With(userWriteRepository).With(candidateWriteRepository).With(authenticationRepository).With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, candidate), Times.Never);

            //Entities were deleted
            candidateWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
            authenticationRepository.Verify(r => r.Delete(candidateId), Times.Once);
            userWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
        }

        [Test]
        public void HardDeleteRemovesUserCandidateAndApplications()
        {
            var dateUpdated = DateTime.UtcNow.AddDays(-14);
            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithStatus(UserStatuses.PendingDeletion).WithDateUpdated(dateUpdated).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var savedSearches = new List<SavedSearch> { new SavedSearchBuilder().Build(), new SavedSearchBuilder().Build() };
            var apprenticeships = new Fixture().Build<ApprenticeshipApplicationSummary>().With(s => s.CandidateId, candidateId).CreateMany(3).ToList();
            var traineeships = new Fixture().Build<TraineeshipApplicationSummary>().With(s => s.CandidateId, candidateId).CreateMany(2).ToList();

            var apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            apprenticeshipApplicationReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(apprenticeships);
            var traineeshipApplicationReadRepository = new Mock<ITraineeshipApplicationReadRepository>();
            traineeshipApplicationReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(traineeships);
            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            var traineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();
            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            savedSearchReadRepository.Setup(r => r.GetForCandidate(candidateId)).Returns(savedSearches);
            var savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
            var authenticationRepository = new Mock<IAuthenticationRepository>();
            var auditRepository = new Mock<IAuditRepository>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new HardDeleteStrategyBuilder()
                .With(apprenticeshipApplicationReadRepository)
                .With(traineeshipApplicationReadRepository)
                .With(userWriteRepository)
                .With(candidateWriteRepository)
                .With(apprenticeshipApplicationWriteRepository)
                .With(traineeshipApplicationWriteRepository)
                .With(savedSearchReadRepository)
                .With(savedSearchWriteRepository)
                .With(auditRepository)
                .With(authenticationRepository)
                .With(successor.Object).Build();

            strategy.Handle(user, candidate);

            //Strategy handled the request
            successor.Verify(s => s.Handle(user, null), Times.Never);

            //Entities were audited
            auditRepository.Verify(r => r.Audit(It.IsAny<object>(), AuditEventTypes.HardDeleteCandidateUser, candidateId, null), Times.Once);

            //Entities were deleted
            foreach (var apprenticeshipApplicationSummary in apprenticeships)
            {
                var summary = apprenticeshipApplicationSummary;
                apprenticeshipApplicationWriteRepository.Verify(r => r.Delete(summary.ApplicationId), Times.Once);
            }
            foreach (var traineeshipApplicationSummary in traineeships)
            {
                var summary = traineeshipApplicationSummary;
                traineeshipApplicationWriteRepository.Verify(r => r.Delete(summary.ApplicationId), Times.Once);
            }
            foreach (var savedSearch in savedSearches)
            {
                var search = savedSearch;
                savedSearchWriteRepository.Verify(r => r.Delete(search.EntityId), Times.Once);
            }
            candidateWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
            authenticationRepository.Verify(r => r.Delete(candidateId), Times.Once);
            userWriteRepository.Verify(r => r.Delete(candidateId), Times.Once);
        }
    }
}