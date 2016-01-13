namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Processes.Applications;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class SubmitTraineeshipApplicationRequestSubscriberTests
    {
        private Mock<ILogService> _mockLogger;

        private Mock<ITraineeshipApplicationReadRepository> _mockTraineeshipApplicationReadRepository;
        private Mock<ITraineeshipApplicationWriteRepository> _mockTraineeshipApplicationWriteRepository;
        private Mock<ICandidateReadRepository> _mockCandidateReadRepository;

        private Mock<ILegacyApplicationProvider> _mockLegacyApplicationProvider;
        private Mock<ILegacyCandidateProvider> _mockLegacyCandidateProvider;

        private SubmitTraineeshipApplicationRequestSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();

            _mockTraineeshipApplicationReadRepository = new Mock<ITraineeshipApplicationReadRepository>();
            _mockTraineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();
            _mockCandidateReadRepository = new Mock<ICandidateReadRepository>();

            _mockLegacyApplicationProvider = new Mock<ILegacyApplicationProvider>();
            _mockLegacyCandidateProvider = new Mock<ILegacyCandidateProvider>();

            _subscriber = new SubmitTraineeshipApplicationRequestSubscriber(
                _mockLogger.Object,
                _mockTraineeshipApplicationReadRepository.Object,
                _mockTraineeshipApplicationWriteRepository.Object,
                _mockCandidateReadRepository.Object,
                _mockLegacyApplicationProvider.Object,
                _mockLegacyCandidateProvider.Object);
        }

        [Test]
        public void ShouldCreateLegacyApplication()
        {
            // Arrange.
            var request = new SubmitTraineeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 5)
                .Create();

            var applicationDetail = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .Create();

            _mockTraineeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            // Act.
            var state = _subscriber.Consume(request);

            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            // Assert.
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Once);
            _mockTraineeshipApplicationWriteRepository.Verify(mock => mock.Save(applicationDetail), Times.Once);
        }

        [TestCase(null)]
        [TestCase(DisabilityStatus.Yes)]
        [TestCase(DisabilityStatus.No)]
        [TestCase(DisabilityStatus.PreferNotToSay)]
        public void ShouldUpdateLegacyCandidate(DisabilityStatus? disabilityStatus)
        {
            // Arrange.
            var request = new SubmitTraineeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 5)
                .Create();

            var applicationDetail = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .With(fixture => fixture.CandidateInformation, new ApplicationTemplate
                {
                    DisabilityStatus = disabilityStatus
                })
                .Create();

            _mockTraineeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            // Act.
            var state = _subscriber.Consume(request);

            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            // Assert.
            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Once);
            candidate.MonitoringInformation.DisabilityStatus.Should().Be(disabilityStatus);
        }

        [Test]
        public void ShouldRequeueRequestWhenLegacyCandidateHasNotBeenCreated()
        {
            // Arrange.
            var request = new SubmitTraineeshipApplicationRequest
            {
                ApplicationId = new Guid()
            };

            var candidate = new Fixture()
                .Build<Candidate>()
                .With(fixture => fixture.LegacyCandidateId, 0)
                .Create();

            var applicationDetail = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, request.ApplicationId)
                .With(fixture => fixture.CandidateId, candidate.EntityId)
                .Create();

            _mockTraineeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            // Act.
            var state = _subscriber.Consume(request);

            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Requeue);

            // Assert.
            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
        }
    }
}