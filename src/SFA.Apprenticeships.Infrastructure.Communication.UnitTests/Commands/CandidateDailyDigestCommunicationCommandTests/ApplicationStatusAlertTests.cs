namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands.CandidateDailyDigestCommunicationCommandTests
{
    using System;
    using Application.Interfaces.Communications;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Processes.Communications.Commands;

    [TestFixture]
    public class ApplicationStatusAlertTests : CandidateCommunicationCommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateDailyDigestCommunicationCommand(
                MessageBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [Test]
        public void ShouldHandleCandidateDailyDigestContainingOneOrMoreApplicationStatusAlerts()
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(1, ApplicationStatuses.Successful)
                .Build();

            var json = JsonConvert.SerializeObject(applicationStatusAlerts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, json)
                .Build();

            // Act.
            var canHandle = Command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeTrue();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldQueueSmsForEachSuccessfulApplicationStatusUpdate(int successfulApplicationCount)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(successfulApplicationCount, ApplicationStatuses.Successful)
                .Build();

            var json = JsonConvert.SerializeObject(applicationStatusAlerts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, json)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueSms(MessageTypes.ApprenticeshipApplicationSuccessful, Times.Exactly(successfulApplicationCount));
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldQueueOneEmailForMultipleSuccessfulApplicationStatusUpdates(int successfulApplicationCount)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(successfulApplicationCount, ApplicationStatuses.Successful)
                .Build();

            var json = JsonConvert.SerializeObject(applicationStatusAlerts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, json)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(MessageTypes.DailyDigest, Times.Once());
        }

        [TestCase(1, MessageTypes.ApprenticeshipApplicationUnsuccessful)]
        [TestCase(2, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        [TestCase(5, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        public void ShouldQueueOneSmsForOneOrMoreUnsuccessfulApplicationStatusUpdates(
            int unsuccessfulApplicationCount, MessageTypes expectedMessageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(unsuccessfulApplicationCount, ApplicationStatuses.Unsuccessful)
                .Build();

            var json = JsonConvert.SerializeObject(applicationStatusAlerts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, json)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueSms(expectedMessageType, Times.Once());
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationUnsuccessful)]
        [TestCase(MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        public void ShouldNotQueueSmsWhenNoApplicationStatusUpdates(MessageTypes expectedMessageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, null)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueSms(expectedMessageType, Times.Never());
        }
    }
}