namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Commands.CandidateCommunication.DailyDigest
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Infrastructure.Processes.Communications.Commands;
    using Builders;
    using NUnit.Framework;
    using JsonConvert = Newtonsoft.Json.JsonConvert;

    [TestFixture]
    [Parallelizable]
    public class ExpiringDraftTests : CommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateDailyDigestCommunicationCommand(
                LogService.Object, ConfigurationService.Object, ServiceBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [Test]
        public void ShouldHandleCandidateDailyDigestContainingOneOrMoreExpiringDraft()
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(1)
                .Build();

            var json = JsonConvert.SerializeObject(expiringDrafts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, json)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, null)
                .Build();

            // Act.
            var canHandle = Command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeTrue();
        }

        [TestCase(1)]
        [TestCase(5)]
        public void ShouldQueueOneEmailForMultipleExpiringDrafts(int expiringDraftCount)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(expiringDraftCount)
                .Build();

            var json = JsonConvert.SerializeObject(expiringDrafts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, json)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, null)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(MessageTypes.DailyDigest, 1);
        }

        [TestCase(1, MessageTypes.ApprenticeshipApplicationExpiringDraft)]
        [TestCase(2, MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary)]
        [TestCase(5, MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary)]
        public void ShouldQueueOneSmsForOneOrMoreExpiringDrafts(
            int expiringDraftCount, MessageTypes expectedMessageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(expiringDraftCount)
                .Build();

            var json = JsonConvert.SerializeObject(expiringDrafts);

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, json)
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, null)
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueSms(expectedMessageType, 1);
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationExpiringDraft)]
        [TestCase(MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary)]
        public void ShouldNotQueueSmsWhenNoExpiringDrafts(MessageTypes expectedMessageType)
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