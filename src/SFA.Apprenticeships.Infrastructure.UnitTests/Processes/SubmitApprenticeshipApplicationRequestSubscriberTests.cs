namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Processes.Applications;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class SubmitApprenticeshipApplicationRequestSubscriberTests
    {
        private Mock<ILogService> _mockLogger;

        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<ICandidateReadRepository> _mockCandidateReadRepository;
        private Mock<IUserReadRepository> _mockUserReadRepository;

        private Mock<ILegacyApplicationProvider> _mockLegacyApplicationProvider;
        private Mock<ILegacyCandidateProvider> _mockLegacyCandidateProvider;

        private SubmitApprenticeshipApplicationRequestSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();

            _mockUserReadRepository = new Mock<IUserReadRepository>();
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockCandidateReadRepository = new Mock<ICandidateReadRepository>();

            _mockLegacyApplicationProvider = new Mock<ILegacyApplicationProvider>();
            _mockLegacyCandidateProvider = new Mock<ILegacyCandidateProvider>();

            _subscriber = new SubmitApprenticeshipApplicationRequestSubscriber(
                _mockLogger.Object,
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockApprenticeshipApplicationWriteRepository.Object,
                _mockCandidateReadRepository.Object,
                _mockUserReadRepository.Object,
                _mockLegacyApplicationProvider.Object,
                _mockLegacyCandidateProvider.Object);
        }

        [Test]
        public void ShouldCreateLegacyApplication()
        {
            // Arrange.
            var request = new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 5)
                .Create();

            var applicationDetail = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.Status, ApplicationStatuses.Submitting)
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            _mockUserReadRepository.Setup(m => m.Get(It.IsAny<Guid>())).Returns(new User {Status = UserStatuses.Active});

            // Act.
            var state = _subscriber.Consume(request);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Once);
            _mockApprenticeshipApplicationWriteRepository.Verify(mock => mock.Save(applicationDetail), Times.Once);

            applicationDetail.Status.Should().Be(ApplicationStatuses.Submitted);
        }

        [TestCase(null)]
        [TestCase(DisabilityStatus.Yes)]
        [TestCase(DisabilityStatus.No)]
        [TestCase(DisabilityStatus.PreferNotToSay)]
        public void ShouldUpdateLegacyCandidate(DisabilityStatus? disabilityStatus)
        {
            // Arrange.
            var request = new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 5)
                .Create();

            var applicationDetail = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.Status, ApplicationStatuses.Submitting)
                .With(fixture => fixture.CandidateInformation, new ApplicationTemplate
                {
                    DisabilityStatus = disabilityStatus
                })
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            _mockUserReadRepository.Setup(m => m.Get(It.IsAny<Guid>())).Returns(new User { Status = UserStatuses.Active });

            // Act.
            var state = _subscriber.Consume(request);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Once);

            candidate.MonitoringInformation.DisabilityStatus.Should().Be(disabilityStatus);
        }

        [Test]
        public void ShouldRequeueRequestWhenLegacyCandidateHasNotBeenCreated()
        {
            // Arrange.
            var request = new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 0)
                .Create();

            var applicationDetail = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.Status, ApplicationStatuses.Submitting)
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            _mockUserReadRepository.Setup(m => m.Get(It.IsAny<Guid>())).Returns(new User { Status = UserStatuses.Active });

            // Act.
            var state = _subscriber.Consume(request);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Requeue);

            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
        }

        [Test]
        public void ShouldNotCreateLegacyApplicationWhenApplicationIsNotAwaitingSubmission()
        {
            // Arrange.
            var request = new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 5)
                .Create();

            var applicationDetail = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.Status, ApplicationStatuses.Draft)
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            _mockUserReadRepository.Setup(m => m.Get(It.IsAny<Guid>())).Returns(new User { Status = UserStatuses.Active });

            // Act.
            var state = _subscriber.Consume(request);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
        }

        [Test]
        public void ShouldNotRequeueIfUserPendingDeletion()
        {
            var request = new SubmitApprenticeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 0)
                .Create();

            var applicationDetail = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.Status, ApplicationStatuses.Draft)
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            _mockUserReadRepository.Setup(m => m.Get(It.IsAny<Guid>())).Returns(new User { Status = UserStatuses.PendingDeletion });

            // Act.
            var state = _subscriber.Consume(request);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);
            
            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
            _mockApprenticeshipApplicationWriteRepository.Verify(a => a.Save(It.IsAny<ApprenticeshipApplicationDetail>()), Times.Once);
        }
    }
}