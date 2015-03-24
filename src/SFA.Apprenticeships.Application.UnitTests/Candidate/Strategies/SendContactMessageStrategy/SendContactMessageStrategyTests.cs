namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SendContactMessageStrategy
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
        private const string HelpdeskEmailAddress = "helpdesk@gmail.com";

        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IContactMessageRepository> _contactMessageRepository = new Mock<IContactMessageRepository>();

        [SetUp]
        public void SetUp()
        {
            _configurationService.Setup(
                cm => cm.Get<UserAccountConfiguration>(UserAccountConfiguration.UserAccountConfigurationName))
                .Returns(new UserAccountConfiguration() {HelpdeskEmailAddress = HelpdeskEmailAddress});
        }

        [Test]
        public void ShouldSendContactMessage()
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
                It.Is<IEnumerable<CommunicationToken>>(ct => ct.Count() == 5 && ct.First().Value == HelpdeskEmailAddress)));
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
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
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