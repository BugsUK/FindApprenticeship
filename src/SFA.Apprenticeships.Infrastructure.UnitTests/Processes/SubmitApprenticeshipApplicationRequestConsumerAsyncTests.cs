namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Logging;
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
    public class SubmitApprenticeshipApplicationRequestConsumerAsyncTests
    {
        private Mock<ILegacyApplicationProvider> _mockLegacyApplicationProvider;
        private Mock<ILegacyCandidateProvider> _mockLegacyCandidateProvider;
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<ICandidateReadRepository> _mockCandidateReadRepository;
        private Mock<IMessageBus> _mockMessageBus;
        private Mock<ILogService> _mockLogger;

        private SubmitApprenticeshipApplicationRequestConsumerAsync _consumer;

        [SetUp]
        public void SetUp()
        {
            _mockLegacyApplicationProvider = new Mock<ILegacyApplicationProvider>();
            _mockLegacyCandidateProvider = new Mock<ILegacyCandidateProvider>();
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockCandidateReadRepository = new Mock<ICandidateReadRepository>();
            _mockMessageBus = new Mock<IMessageBus>();
            _mockLogger = new Mock<ILogService>();

            _consumer = new SubmitApprenticeshipApplicationRequestConsumerAsync(
                _mockLegacyApplicationProvider.Object,
                _mockLegacyCandidateProvider.Object,
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockApprenticeshipApplicationWriteRepository.Object,
                _mockCandidateReadRepository.Object,
                _mockMessageBus.Object,
                _mockLogger.Object);
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

            // Act.
            _consumer.Consume(request).Wait();

            // Assert.
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Once);
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
                    MonitoringInformation = new MonitoringInformation
                    {
                        DisabilityStatus = disabilityStatus
                    }
                })
                .Create();

            _mockApprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(applicationDetail.EntityId, true))
                .Returns(applicationDetail);

            _mockCandidateReadRepository.Setup(mock => mock
                .Get(candidate.EntityId, true))
                .Returns(candidate);

            // Act.
            _consumer.Consume(request).Wait();

            // Assert.
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

            // Act.
            _consumer.Consume(request).Wait();

            // Assert.
            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
            _mockMessageBus.Verify(mock => mock.PublishMessage(request), Times.Once);
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

            // Act.
            _consumer.Consume(request).Wait();

            // Assert.
            _mockLegacyCandidateProvider.Verify(mock => mock.UpdateCandidate(candidate), Times.Never);
            _mockLegacyApplicationProvider.Verify(mock => mock.CreateApplication(applicationDetail), Times.Never);
            _mockMessageBus.Verify(mock => mock.PublishMessage(request), Times.Never);
        }
    }
}