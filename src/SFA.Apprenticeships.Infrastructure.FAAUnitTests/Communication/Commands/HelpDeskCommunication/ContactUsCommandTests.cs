namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Commands.HelpDeskCommunication
{
    using System.Linq;
    using Application.Interfaces.Communications;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Infrastructure.Processes.Communications.Commands;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ContactUsCommandTests
    {
        private const string TestEmailAddress = "jane.doe@example.com";

        private Mock<IServiceBus> _mockServiceBus;
        private CommunicationRequest _communicationRequest;

        [SetUp]
        public void SetUp()
        {
            _mockServiceBus = new Mock<IServiceBus>();

            _communicationRequest = new CommunicationRequest
            {
                MessageType = MessageTypes.CandidateContactUsMessage,
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
        public void ShouldBeAbleToHandleCandidateContactUsMessage()
        {
            // Act.
            var command = new HelpDeskCommunicationCommand(null);

            // Assert.
            command.CanHandle(_communicationRequest).Should().BeTrue();
        }

        [TestCase(MessageTypes.ApprenticeshipApplicationSubmitted)]
        [TestCase(MessageTypes.DailyDigest)]
        [TestCase(MessageTypes.PasswordChanged)]
        [TestCase(MessageTypes.SendAccountUnlockCode)]
        [TestCase(MessageTypes.SendActivationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCode)]
        [TestCase(MessageTypes.SendMobileVerificationCodeReminder)]
        [TestCase(MessageTypes.SendPasswordResetCode)]
        [TestCase(MessageTypes.TraineeshipApplicationSubmitted)]
        [TestCase(MessageTypes.SendPendingUsernameCode)]
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
            var command = new HelpDeskCommunicationCommand(_mockServiceBus.Object);
            // Act.
            command.Handle(_communicationRequest);

            // Assert.
            _mockServiceBus.Verify(mock => mock.PublishMessage(
                It.Is<EmailRequest>(emailRequest =>
                    emailRequest.MessageType == _communicationRequest.MessageType &&
                    emailRequest.ToEmail == TestEmailAddress &&
                    emailRequest.Tokens.Count() == 3)),
                Times.Once);
        }

        [Test]
        public void ShouldNotQueueSms()
        {
            // Arrange.
            var command = new HelpDeskCommunicationCommand(_mockServiceBus.Object);

            // Act.
            command.Handle(_communicationRequest);

            // Assert.
            _mockServiceBus.Verify(mock => mock.PublishMessage(It.IsAny<SmsRequest>()), Times.Never);
        }
    }
}