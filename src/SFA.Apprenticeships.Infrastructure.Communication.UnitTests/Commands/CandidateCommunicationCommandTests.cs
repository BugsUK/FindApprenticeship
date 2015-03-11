namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands
{
    using System;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Communications.Commands;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateCommunicationCommandTests
    {
        private const string TestEmailAddress = "jane.doe@example.com";
        private const string TestMobileNumber = "07999999999";

        private Mock<IMessageBus> _messageBus;
        private CandidateCommunicationCommand _command;
        private Mock<ICandidateReadRepository> _candidateRepository;
        private Mock<IUserReadRepository> _userRepository;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new Mock<IMessageBus>();
            _userRepository = new Mock<IUserReadRepository>();
            _candidateRepository = new Mock<ICandidateReadRepository>();
            _command = new CandidateCommunicationCommand(_messageBus.Object, _candidateRepository.Object, _userRepository.Object);
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted)]
        [TestCase(MessageTypes.DailyDigest)]
        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCode)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted)]
        public void ShouldHandleCandidateCommunicationMessagesTypes(MessageTypes messageType)
        {
            // Arrange.
            var communicationRequest = new CommunicationRequest
            {
                MessageType = messageType,
                Tokens = new[]
                {
                    new CommunicationToken(CommunicationTokens.RecipientEmailAddress, TestEmailAddress)
                }
            };

            // Act.
            var canHandle = _command.CanHandle(communicationRequest);

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
            Action action = () => _command.Handle(communicationRequest);

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [TestCase(MessageTypes.CandidateContactMessage)]
        public void ShouldNotBeAbleToHandleNonCandidateCommunicationMessageTypes(MessageTypes messageType)
        {
            // Arrange.
            var communicationRequest = new CommunicationRequest
            {
                MessageType = messageType
            };

            // Act.
            var canHandle = _command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeFalse();
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted, UserStatuses.Active)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted, UserStatuses.Active)]
        [TestCase(MessageTypes.DailyDigest, UserStatuses.Active)]
        [TestCase(MessageTypes.DailyDigest, UserStatuses.Locked)]
        [TestCase(MessageTypes.DailyDigest, UserStatuses.PendingActivation)]
        public void ShouldQueueEmailAndSmsForActiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate, userStatus);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            _command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(communicationRequest, Times.Once());
            ShouldQueueSms(communicationRequest, Times.Once());
        }

        [TestCase(MessageTypes.DailyDigest, UserStatuses.Inactive)]
        [TestCase(MessageTypes.DailyDigest, UserStatuses.Dormant)]
        public void ShouldNotQueueEmailOrSmsForInactiveCandidate(MessageTypes messageType, UserStatuses userStatus)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate, userStatus);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            _command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(communicationRequest, Times.Never());
            ShouldQueueSms(communicationRequest, Times.Never());
        }

        [TestCase(MessageTypes.DailyDigest)]
        public void ShouldNotQueueSmsIfUnverifiedMobile(MessageTypes messageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(false)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            _command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(communicationRequest, Times.Once());
            ShouldQueueSms(communicationRequest, Times.Never());
        }

        [TestCase(MessageTypes.SendMobileVerificationCode, true)]
        [TestCase(MessageTypes.SendMobileVerificationCode, false)]
        public void ShouldNotQueueEmailMessageForSmsOnlyMessageType(MessageTypes messageType, bool verifiedMobile)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(verifiedMobile)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            _command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(communicationRequest, Times.Never());
            ShouldQueueSms(communicationRequest, Times.Once());
        }

        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        public void ShouldNotQueueSmsForNonSmsMessageType(MessageTypes messageType)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowEmail(true)
                .AllowMobile(true)
                .VerifiedMobile(true)
                .Build();

            AddCandidate(candidate);

            var communicationRequest = new CommunicationRequestBuilder(messageType, candidate.EntityId).Build();

            // Act.
            _command.Handle(communicationRequest);

            // Assert.
            ShouldQueueEmail(communicationRequest, Times.Once());
            ShouldQueueSms(communicationRequest, Times.Never());
        }

        #region Helpers

        private void ShouldQueueSms(CommunicationRequest communicationRequest, Times times)
        {
            _messageBus.Verify(mock => mock.PublishMessage(
                It.Is<SmsRequest>(smsRequest =>
                    smsRequest.MessageType == communicationRequest.MessageType &&
                    smsRequest.ToNumber == TestMobileNumber &&
                    !smsRequest.Tokens.Any())),
                times);
        }

        private void ShouldQueueEmail(CommunicationRequest communicationRequest, Times times)
        {
            _messageBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == communicationRequest.MessageType &&
                    !emailRequest.Tokens.Any())),
                times);
        }

        private void AddCandidate(Candidate candidate, UserStatuses userStatus = UserStatuses.Active)
        {
            var user = new UserBuilder(candidate.EntityId)
                .WithStatus(userStatus)
                .Build();

            _candidateRepository
                .Setup(mock => mock.Get(candidate.EntityId))
                .Returns(candidate);

            _userRepository
                .Setup(mock => mock.Get(candidate.EntityId))
                .Returns(user);
        }

        private class CommunicationRequestBuilder
        {
            private readonly MessageTypes _messageType;
            private readonly Guid _candidateId;

            public CommunicationRequestBuilder(MessageTypes messageType, Guid candidateId)
            {
                _messageType = messageType;
                _candidateId = candidateId;
            }

            public CommunicationRequest Build()
            {
                return new CommunicationRequest
                {
                    MessageType = _messageType,
                    Tokens = new[]
                    {
                        new CommunicationToken(CommunicationTokens.RecipientEmailAddress, TestEmailAddress),
                        new CommunicationToken(CommunicationTokens.CandidateMobileNumber, TestMobileNumber)
                    },
                    EntityId = _candidateId
                };
            }
        }

        #endregion
    }
}