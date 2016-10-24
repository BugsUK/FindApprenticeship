namespace SFA.Apprenticeships.Application.UnitTests.Application.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Apprenticeships.Application.Application.Entities;
    using SFA.Apprenticeships.Application.Application.Strategies;

    [TestFixture]
    public class SetApplicationStatusStrategyTests
    {
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<IReferenceNumberRepository> _mockReferenceNumberRepository;
        private Mock<IApplicationStatusUpdateStrategy> _mockApplicationStatusUpdateStrategy;
        private Mock<IServiceBus> _mockServiceBus;

        private SetApplicationStatusStrategy _setApplicationStatusStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockReferenceNumberRepository = new Mock<IReferenceNumberRepository>();
            _mockApplicationStatusUpdateStrategy = new Mock<IApplicationStatusUpdateStrategy>();
            _mockServiceBus = new Mock<IServiceBus>();

            _mockApprenticeshipApplicationReadRepository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(new Fixture().Create<ApprenticeshipApplicationDetail>());

            _setApplicationStatusStrategy = new SetApplicationStatusStrategy(_mockApprenticeshipApplicationReadRepository.Object, _mockApprenticeshipApplicationWriteRepository.Object, _mockReferenceNumberRepository.Object, _mockApplicationStatusUpdateStrategy.Object, _mockServiceBus.Object);
        }

        [Test]
        public void SetStateSubmitted_PostMessage()
        {
            var applicationId = Guid.NewGuid();

            _setApplicationStatusStrategy.SetStateSubmitted(applicationId);

            _mockServiceBus.Verify(sb => sb.PublishMessage(It.Is<ApprenticeshipApplicationUpdate>(m => m.ApplicationGuid == applicationId)));
        }

        [Test]
        public void SetStateInProgress_PostMessage()
        {
            var applicationId = Guid.NewGuid();

            _setApplicationStatusStrategy.SetStateInProgress(applicationId);

            _mockServiceBus.Verify(sb => sb.PublishMessage(It.Is<ApprenticeshipApplicationUpdate>(m => m.ApplicationGuid == applicationId)));
        }

        [Test]
        public void SetSuccessfulDecision_PostMessage()
        {
            var applicationId = Guid.NewGuid();

            _setApplicationStatusStrategy.SetSuccessfulDecision(applicationId);

            _mockServiceBus.Verify(sb => sb.PublishMessage(It.Is<ApprenticeshipApplicationUpdate>(m => m.ApplicationGuid == applicationId)));
        }

        [Test]
        public void SetUnsuccessfulDecision_PostMessage()
        {
            var applicationId = Guid.NewGuid();

            _setApplicationStatusStrategy.SetUnsuccessfulDecision(applicationId, "Reason");

            _mockServiceBus.Verify(sb => sb.PublishMessage(It.Is<ApprenticeshipApplicationUpdate>(m => m.ApplicationGuid == applicationId)));
        }
    }
}