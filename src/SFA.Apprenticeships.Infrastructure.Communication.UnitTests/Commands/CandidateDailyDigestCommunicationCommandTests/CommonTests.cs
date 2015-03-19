﻿namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands.CandidateDailyDigestCommunicationCommandTests
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
    public class CommonTests : CandidateCommunicationCommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateDailyDigestCommunicationCommand(
                MessageBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [Test]
        public void ShouldHandleCandidateDailyDigestContainingOneOrMoreExpiringDraftsAndApplicationStatusAlerts()
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(1)
                .Build();

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(1, ApplicationStatuses.Unsuccessful)
                .Build();

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, JsonConvert.SerializeObject(expiringDrafts))
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, JsonConvert.SerializeObject(applicationStatusAlerts))
                .Build();

            // Act.
            var canHandle = Command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeTrue();
        }

        [TestCase(1, 1)]
        [TestCase(5, 5)]
        public void ShouldQueueOneEmailForMultipleExpiringDraftsAndApplicationStatusAlerts(
            int expiringDraftCount, int applicationStatusAlertCount)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(expiringDraftCount)
                .Build();

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(applicationStatusAlertCount, ApplicationStatuses.Unsuccessful)
                .Build();

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts, JsonConvert.SerializeObject(expiringDrafts))
                .WithToken(CommunicationTokens.ApplicationStatusAlerts, JsonConvert.SerializeObject(applicationStatusAlerts))
                .Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(MessageTypes.DailyDigest, Times.Once());
        }
    }
}