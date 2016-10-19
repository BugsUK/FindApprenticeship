namespace SFA.Apprenticeships.Application.UnitTests.Application.Strategies.Apprenticeships
{
    using System;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Application.Entities;
    using SFA.Apprenticeships.Application.Application.Strategies.Apprenticeships;

    [TestFixture]
    public class UpdateApplicationNotesStrategyTests
    {
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<IServiceBus> _mockServiceBus;

        private UpdateApplicationNotesStrategy _updateApplicationNotesStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockServiceBus = new Mock<IServiceBus>();

            _updateApplicationNotesStrategy = new UpdateApplicationNotesStrategy(_mockApprenticeshipApplicationWriteRepository.Object, _mockServiceBus.Object);
        }

        [Test]
        public void UpdateApplicationNotes_PostMessage()
        {
            var applicationId = Guid.NewGuid();

            _updateApplicationNotesStrategy.UpdateApplicationNotes(applicationId, "Note");

            _mockServiceBus.Verify(sb => sb.PublishMessage(It.Is<ApprenticeshipApplicationUpdate>(m => m.ApplicationGuid == applicationId)));
        }
    }
}