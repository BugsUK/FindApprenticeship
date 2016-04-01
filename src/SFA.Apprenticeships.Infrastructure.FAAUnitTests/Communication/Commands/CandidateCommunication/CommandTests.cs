namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Commands.CandidateCommunication
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Builders;
    using FluentAssertions;
    using Infrastructure.Processes.Communications.Commands;
    using NUnit.Framework;

    [TestFixture]
    public class CommandTests : CommandTestsBase
    {
        [SetUp]
        public void SetUp()
        {
            var command = new CandidateCommunicationCommand(
                LogService.Object, ConfigurationService.Object, ServiceBus.Object, CandidateRepository.Object, UserRepository.Object);

            base.SetUp(command);
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted)]
        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCodeReminder)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted)]
        [TestCase(MessageTypes.SendPendingUsernameCode)]
        public void ShouldHandleSimpleCandidateMessagesTypes(MessageTypes messageType)
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

        [TestCase(MessageTypes.CandidateContactUsMessage)]
        [TestCase(MessageTypes.CandidateFeedbackMessage)]
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
        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Dormant)]
        public void ShouldQueueEmailAndSmsForActiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .Build();

            AddCandidate(candidate, userStatus);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            Command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(messageType, 1);
            if (messageType == MessageTypes.ApprenticeshipApplicationSubmitted || messageType == MessageTypes.TraineeshipApplicationSubmitted)
            {
                ShouldQueueSms(messageType, 0);
            }
            else
            {
                ShouldQueueSms(messageType, 1);
            }
        }

        [TestCase(MessageTypes.SavedSearchAlert, UserStatuses.Inactive)]
        public void ShouldNotQueueEmailOrSmsForInactiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
                .EnableAllCommunications()
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
        [TestCase(MessageTypes.SendMobileVerificationCodeReminder, true)]
        [TestCase(MessageTypes.SendMobileVerificationCodeReminder, false)]
        public void ShouldNotQueueEmailMessageForSmsOnlyMessageType(MessageTypes messageType, bool verifiedMobile)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
        [TestCase(MessageTypes.SendPendingUsernameCode)]
        public void ShouldNotQueueSmsForNonSmsMessageType(MessageTypes messageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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