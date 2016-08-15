namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Commands.CandidateCommunication.DailyDigest
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.Applications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Infrastructure.Processes.Communications.Commands;
    using Builders;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApplicationStatusAlertTests : CommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateDailyDigestCommunicationCommand(
                LogService.Object, ConfigurationService.Object, ServiceBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [Test]
        public void ShouldHandleCandidateDailyDigestContainingOneOrMoreApplicationStatusAlerts()
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
                .EnableAllCommunications()
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
            ShouldQueueSms(MessageTypes.ApprenticeshipApplicationSuccessful, successfulApplicationCount);
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldQueueOneEmailForMultipleSuccessfulApplicationStatusUpdates(int successfulApplicationCount)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
            ShouldQueueEmail(MessageTypes.DailyDigest, 1);
        }

        [TestCase(1, MessageTypes.ApprenticeshipApplicationUnsuccessful)]
        [TestCase(2, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        [TestCase(5, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        public void ShouldQueueOneSmsForOneOrMoreUnsuccessfulApplicationStatusUpdates(
            int unsuccessfulApplicationCount, MessageTypes expectedMessageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
            ShouldQueueSms(expectedMessageType, 1);
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationUnsuccessful)]
        [TestCase(MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary)]
        public void ShouldNotQueueSmsWhenNoApplicationStatusUpdates(MessageTypes expectedMessageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, null)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, null)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueSms(expectedMessageType, 0);
        }
    }
}