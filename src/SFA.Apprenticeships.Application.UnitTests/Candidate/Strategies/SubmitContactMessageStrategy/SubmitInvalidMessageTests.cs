namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SubmitContactMessageStrategy
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class SubmitInvalidMessageTests
    {
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IContactMessageRepository> _contactMessageRepository = new Mock<IContactMessageRepository>();
        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();

        private SubmitContactMessageStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new SubmitContactMessageStrategy(
                _logService.Object, _communicationService.Object, _configurationService.Object, _contactMessageRepository.Object);
        }


        [Test]
        public void ShouldNotSendInvalidContactMessage()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Unknown
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _logService.Verify(mock => mock.Error(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);

            _communicationService.Verify(mock => mock.SendContactMessage(
                It.IsAny<Guid?>(),
                MessageTypes.CandidateContactUsMessage,
                It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
        }

        [Test]
        public void ShouldSaveInvalidMessage()
        {
            // Arrange.
            var contactMessage = new ContactMessage
            {
                Type = ContactMessageTypes.Unknown,
                UserId = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "jane.doe@example.com",
                Enquiry = "Enquiry",
                Details = "Details"
            };

            // Act.
            _strategy.SubmitMessage(contactMessage);

            // Assert.
            _contactMessageRepository.Verify(mock => mock.Save(contactMessage), Times.Once);
        }
    }
}