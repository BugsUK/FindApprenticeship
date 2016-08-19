namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SubmitContactMessageStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidate.Strategies;
    using Apprenticeships.Application.UserAccount.Configuration;
    using Domain.Entities.Communication;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class SubmitContactUsMessageTests
    {
        private const string ValidHelpdeskEmailAddress = "helpdesk@example.com";
        private const string ValidUserFullName = "Jane Doe";
        private const string ValidUserEnquiryDetails = "Some details";
        private const string ValidUserEmailAddress = "jane.doe@example.com";

        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IContactMessageRepository> _contactMessageRepository = new Mock<IContactMessageRepository>();
        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();

        private SubmitContactMessageStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _configurationService.Setup(
                cm => cm.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration
                {
                    HelpdeskEmailAddress = ValidHelpdeskEmailAddress
                });

            _strategy = new SubmitContactMessageStrategy(
                _logService.Object, _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);
        }

        [Test]
        public void ShouldSendContactUsMessageToHelpDeskEmailAddress()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.ContactUs
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(), 
                MessageTypes.CandidateContactUsMessage, 
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.RecipientEmailAddress).Value == ValidHelpdeskEmailAddress)));
        }

        [Test]
        public void ShouldSendContactUsMessageWithUserFullName()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.ContactUs,
                Name = ValidUserFullName
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactUsMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserFullName).Value == ValidUserFullName)));
        }
        
        [Test]
        public void ShouldSendContactUsMessageWithUserEmailAddress()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.ContactUs,
                Email = ValidUserEmailAddress
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactUsMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEmailAddress).Value == ValidUserEmailAddress)));
        }

        [TestCase(null, SubmitContactMessageStrategy.DefaultUserEnquiryDetails)]
        [TestCase(ValidUserEnquiryDetails, ValidUserEnquiryDetails)]
        public void ShouldSendContactUsMessageWithUserEnquiryDetails(string details, string expectedUserEnquiryDetails)
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.ContactUs,
                Details = details
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactUsMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEnquiryDetails).Value == expectedUserEnquiryDetails)));
        }

        [Test]
        public void ShouldSaveContactUsMessage()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.ContactUs,
                UserId = Guid.NewGuid(),
                Name = ValidUserFullName,
                Email = ValidUserEmailAddress,
                Enquiry = "I've forgotten my password",
                Details = "I've still forgotten my password"
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _contactMessageRepository.Verify(mock => mock.Save(contactMessage), Times.Once);
        }
    }
}