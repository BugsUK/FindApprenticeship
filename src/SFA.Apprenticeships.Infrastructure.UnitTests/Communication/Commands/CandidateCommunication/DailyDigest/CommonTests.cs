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
    public class CommonTests : CommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateDailyDigestCommunicationCommand(
                LogService.Object, ConfigurationService.Object, ServiceBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [Test]
        public void ShouldHandleCandidateDailyDigestContainingOneOrMoreExpiringDraftsAndApplicationStatusAlerts()
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate);

            var expiringDrafts = new ExpiringApprenticeshipApplicationDraftsBuilder()
                .WithExpiringDrafts(1)
                .Build();

            var applicationStatusAlerts = new ApplicationStatusAlertsBuilder()
                .WithApplicationStatusAlerts(1, ApplicationStatuses.Unsuccessful)
                .Build();

            var communicationRequest = new CommunicationRequestBuilder(MessageTypes.DailyDigest, candidate.EntityId)
                .WithToken(CommunicationTokens.ExpiringDrafts,  JsonConvert.SerializeObject(expiringDrafts))
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
                .EnableAllCommunications()
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
            ShouldQueueEmail(MessageTypes.DailyDigest, 1);
        }
    }
}