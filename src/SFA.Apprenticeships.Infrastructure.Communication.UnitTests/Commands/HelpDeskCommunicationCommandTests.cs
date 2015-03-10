namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Commands
{
    using System.Linq;
    using Application.Interfaces.Communications;
    using Communications.Commands;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HelpDeskCommunicationCommandTests
    {
        private const string TestEmailAddress = "jane.doe@example.com";
        private const string TestActivationCode = "XYZ789";

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
                    new CommunicationToken(CommunicationTokens.ActivationCode, TestActivationCode)
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

        [Test]
        public void ShouldBeUnableToHandleOtherTypeOfMessage()
        {
            // Arrange.
            var communicationRequest = new CommunicationRequest
            {
                MessageType = MessageTypes.SendActivationCode
            };

            // Act.
            var command = new HelpDeskCommunicationCommand(null);

            // Assert.
            command.CanHandle(communicationRequest).Should().BeFalse();
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
                    emailRequest.Tokens.Count() == 1)),
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