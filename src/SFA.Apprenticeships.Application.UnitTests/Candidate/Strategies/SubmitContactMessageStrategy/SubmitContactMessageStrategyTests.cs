namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SubmitContactMessageStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.UserAccount.Configuration;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendContactMessageStrategyTests
    {
        private const string ValidHelpdeskEmailAddress = "helpdesk@example.com";
        private const string ValidUserFullName = "Jane Doe";
        private const string ValidUserEnquiryDetails = "Bacon ipsum";
        private const string ValidUserEmailAddress = "jane.doe@example.com";

        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IContactMessageRepository> _contactMessageRepository = new Mock<IContactMessageRepository>();

        [SetUp]
        public void SetUp()
        {
            _configurationService.Setup(
                cm => cm.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration
                {
                    HelpdeskEmailAddress = ValidHelpdeskEmailAddress
                });
        }

        [Test]
        public void ShouldSendContactMessageToHelpDeskEmailAddress()
        {
            // Arrange.
            var strategy = new Application.Candidate.Strategies.SubmitContactMessageStrategy(
                _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);

            // Act.
            strategy.SubmitMessage(new ContactMessage());

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(), 
                MessageTypes.CandidateContactMessage, 
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.RecipientEmailAddress).Value == ValidHelpdeskEmailAddress)));
        }

        [TestCase(null, Application.Candidate.Strategies.SubmitContactMessageStrategy.DefaultUserFullName)]
        [TestCase(ValidUserFullName, ValidUserFullName)]
        public void ShouldSendContactMessageWithUserFullName(string name, string expectedUserFullName)
        {
            // Arrange.
            var strategy = new Application.Candidate.Strategies.SubmitContactMessageStrategy(
                _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);

            // Act.
            strategy.SubmitMessage(new ContactMessage
            {
                Name = name
            });

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserFullName).Value == expectedUserFullName)));
        }

        [TestCase(null, Application.Candidate.Strategies.SubmitContactMessageStrategy.DefaultUserEmailAddress)]
        [TestCase(ValidUserEmailAddress, ValidUserEmailAddress)]
        public void ShouldSendContactMessageWithUserEmailAddress(string email, string expectedUserEmailAddress)
        {
            // Arrange.
            var strategy = new Application.Candidate.Strategies.SubmitContactMessageStrategy(
                _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);

            // Act.
            strategy.SubmitMessage(new ContactMessage
            {
                Email = email
            });

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEmailAddress).Value == expectedUserEmailAddress)));
        }

        [TestCase(null, Application.Candidate.Strategies.SubmitContactMessageStrategy.DefaultUserEnquiryDetails)]
        [TestCase(ValidUserEnquiryDetails, ValidUserEnquiryDetails)]
        public void ShouldSendContactMessageWithUserEnquiryDetails(string details, string expectedUserEnquiryDetails)
        {
            // Arrange.
            var strategy = new Application.Candidate.Strategies.SubmitContactMessageStrategy(
                _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);

            // Act.
            strategy.SubmitMessage(new ContactMessage
            {
                Details = details
            });

            // Assert.
            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactMessage,
                It.Is<IEnumerable<CommunicationToken>>(communicationTokens => communicationTokens.First(
                    each => each.Key == CommunicationTokens.UserEnquiryDetails).Value == expectedUserEnquiryDetails)));
        }

        [Test]
        public void ShouldSaveContactMessage()
        {
            // Arrange.
            var strategy = new Application.Candidate.Strategies.SubmitContactMessageStrategy(
                _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);

            var contactMessage = new ContactMessage
            {
                UserId = Guid.NewGuid(),
                Name = ValidUserFullName,
                Email = ValidUserEmailAddress,
                Enquiry = "I've forgotten my password",
                Details = "I've still forgotten my password"
            };

            // Act.
            strategy.SubmitMessage(contactMessage);

            // Assert.
            _contactMessageRepository.Verify(repo => repo.Save(contactMessage), Times.Once());
        }
    }
}