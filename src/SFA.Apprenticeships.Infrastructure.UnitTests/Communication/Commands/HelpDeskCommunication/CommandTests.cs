namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Commands.HelpDeskCommunication
{
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Infrastructure.Processes.Communications.Commands;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CommandTests
    {
        private const string TestEmailAddress = "jane.doe@example.com";

        private Mock<IMessageBus> _messageBus;
        private CommunicationRequest _candidateContactCommunicationRequest;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new Mock<IMessageBus>();

            _candidateContactCommunicationRequest = new CommunicationRequest
            {
                MessageType = MessageTypes.CandidateContactMessage,
                Tokens = new[]
                {
                    new CommunicationToken(CommunicationTokens.RecipientEmailAddress, TestEmailAddress),
                    new CommunicationToken(CommunicationTokens.UserFullName, "Jane Doe"),
                    new CommunicationToken(CommunicationTokens.UserEnquiry, "I have forgotten my password"),
                    new CommunicationToken(CommunicationTokens.UserEnquiryDetails, "I have still forgotten my password")
                }
            };
        }

        [Test]
        public void ShouldBeAbleToHandleCandidateContactMessage()
        {
            // Act.
            var command = new HelpDeskCommunicationCommand(null);

            // Assert.
            command.CanHandle(_candidateContactCommunicationRequest).Should().BeTrue();
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted)]
        [TestCase(MessageTypes.DailyDigest)]
        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCode)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted)]
        public void ShouldBeUnableToHandleOtherTypeOfMessage(MessageTypes messageType)
        {
            // Arrange.
            var communicationRequest = new CommunicationRequest
            {
                MessageType = messageType
            };

            var command = new HelpDeskCommunicationCommand(null);

            // Act.
            var canHandle = command.CanHandle(communicationRequest);

            // Assert.
            canHandle.Should().BeFalse();
        }

        [Test]
        public void ShouldQueueEmail()
        {
            // Arrange.
            var command = new HelpDeskCommunicationCommand(_messageBus.Object);
            // Act.
            command.Handle(_candidateContactCommunicationRequest);

            // Assert.
            _messageBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == _candidateContactCommunicationRequest.MessageType &&
                    emailRequest.ToEmail == TestEmailAddress &&
                    emailRequest.Tokens.Count() == 3)),
                Times.Once);
        }

        [Test]
        public void ShouldNotQueueSms()
        {
            // Arrange.
            var command = new HelpDeskCommunicationCommand(_messageBus.Object);

            // Act.
            command.Handle(_candidateContactCommunicationRequest);

            // Assert.
            _messageBus.Verify(mock => mock.PublishMessage(It.IsAny<SmsRequest>()), Times.Never);
        }
    }
}