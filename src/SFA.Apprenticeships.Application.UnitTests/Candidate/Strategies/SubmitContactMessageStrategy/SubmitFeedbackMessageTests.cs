namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SubmitContactMessageStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidate.Strategies;
    using Apprenticeships.Application.UserAccount.Configuration;
    using Domain.Entities.Communication;
    using Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitFeedbackMessageTests
    {
        private const string ValidFeedbackEmailAddress = "feedback@example.com";
        private const string ValidNoReplyEmailAddress = "noreply@example.com";
        private const string ValidUserFullName = "Jane Doe";
        private const string ValidUserEmailAddress = "jane.doe@example.com";

        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IContactMessageRepository> _contactMessageRepository = new Mock<IContactMessageRepository>();

        private SubmitContactMessageStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _configurationService.Setup(
                cm => cm.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration
                {
                    FeedbackEmailAddress = ValidFeedbackEmailAddress,
                    NoReplyEmailAddress = ValidNoReplyEmailAddress
                });

            _strategy = new SubmitContactMessageStrategy(
                _logService.Object, _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);
        }

        [Test]
        public void ShouldSendFeedbackMessageToFeedbackEmailAddress()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Feedback
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(), 
                MessageTypes.CandidateFeedbackMessage, 
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.RecipientEmailAddress).Value == ValidFeedbackEmailAddress)));
        }

        [TestCase(null, SubmitContactMessageStrategy.DefaultUserFullName)]
        [TestCase(ValidUserFullName, ValidUserFullName)]
        public void ShouldSendFeedbackMessageWithUserFullName(string name, string expectedUserFullName)
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Feedback,
                Name = name
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateFeedbackMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserFullName).Value == expectedUserFullName)));
        }

        [TestCase(null, ValidNoReplyEmailAddress)]
        [TestCase(ValidUserEmailAddress, ValidUserEmailAddress)]
        public void ShouldSendFeedbackMessageEmailAddress(string emailAddress, string expectedEmailAddress)
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Feedback,
                Email = emailAddress
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateFeedbackMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEmailAddress).Value == expectedEmailAddress)));
        }

        [Test]
        public void ShouldSendFeedbackMessageWithDefaultUserEnquiryDetails()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Feedback,
                Details = "Some details"
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateFeedbackMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEnquiryDetails).Value == contactMessage.Details)));
        }

        [Test]
        public void ShouldSaveFeedbackMessage()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Feedback,
                UserId = Guid.NewGuid(),
                Name = ValidUserFullName,
                Email = ValidUserEmailAddress,
                Details = "Some feedback"
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _contactMessageRepository.Verify(mock => mock.Save(contactMessage), Times.Once);
        }
    }
}