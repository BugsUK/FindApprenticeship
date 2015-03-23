namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands.CandidateCommunication
{
    using System;
    using Application.Interfaces.Communications;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using FluentAssertions;
    using NUnit.Framework;
    using Processes.Communications.Commands;

    [TestFixture]
    public class CommandTests : CommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateCommunicationCommand(
                MessageBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted)]
        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCode)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted)]
        public void ShouldHandleMostCandidateMessagesTypes(MessageTypes messageType)
        {
            // Arrange.
            var communicationRequest = new CommunicationRequestBuilder(messageType, Guid.NewGuid()).Build();

            // Act.
            var canHandle = Command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeTrue();
        }

        [TestCase]
        public void ShouldThrowIfCandidateIdIsNull()
        {
            // Arrange.
            var communicationRequest = new CommunicationRequest
            {
                MessageType = MessageTypes.SendActivationCode
            };

            // Act.
            Action action = () => Command.Handle(communicationRequest);

            // Assert.
            action.ShouldThrowExactly<InvalidOperationException>();
        }

        [TestCase(MessageTypes.CandidateContactMessage)]
        [TestCase(MessageTypes.DailyDigest)]
        public void ShouldNotBeAbleToHandleOtherMessageTypes(MessageTypes messageType)
        {
            // Arrange.
            var communicationRequest = new CommunicationRequestBuilder(messageType, Guid.NewGuid()).Build();

            // Act.
            var canHandle = Command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeFalse();
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted, UserStatuses.Active)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted, UserStatuses.Active)]
        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Active)]
        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Locked)]
        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.PendingActivation)]
        public void ShouldQueueEmailAndSmsForActiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .Build();

            AddCandidate(candidate, userStatus);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 1);
            ShouldQueueSms(messageType, 1);
        }

        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Inactive)]
        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Dormant)]
        public void ShouldNotQueueEmailOrSmsForInactiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .Build();

            AddCandidate(candidate, userStatus);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 0);
            ShouldQueueSms(messageType, 0);
        }

        [TestCase(MessageTypes.SavedSearchAlert)]
        public void ShouldNotQueueSmsIfUnverifiedMobile(MessageTypes messageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .VerifiedMobile(false)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 1);
            ShouldQueueSms(messageType, 0);
        }

        [TestCase(MessageTypes.SendMobileVerificationCode, true)]
        [TestCase(MessageTypes.SendMobileVerificationCode, false)]
        public void ShouldNotQueueEmailMessageForSmsOnlyMessageType(MessageTypes messageType, bool verifiedMobile)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .VerifiedMobile(verifiedMobile)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 0);
            ShouldQueueSms(messageType, 1);
        }

        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        public void ShouldNotQueueSmsForNonSmsMessageType(MessageTypes messageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 1);
            ShouldQueueSms(messageType, 0);
        }
    }
}