namespace SFA.Apprenticeships.Application.UnitTests.Strategies
{
    using System;
    using Application.Applications.Strategies;
    using Application.Candidates;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class AuditApplicationDetailStrategyTests
    {
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository;
        private Mock<ITraineeshipApplicationReadRepository> _traineeshipApplicationReadRepository;
        private Mock<IAuditRepository> _auditRepository;

        private AuditApplicationDetailStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _traineeshipApplicationReadRepository = new Mock<ITraineeshipApplicationReadRepository>();

            _auditRepository = new Mock<IAuditRepository>();

            _strategy = new AuditApplicationDetailStrategy(
                _apprenticeshipApplicationReadRepository.Object,
                _traineeshipApplicationReadRepository.Object,
                _auditRepository.Object);
        }

        [Test]
        public void ShouldAuditApprenticeshipApplication()
        {
            // Arrange.
            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .Create();

            _apprenticeshipApplicationReadRepository.Setup(mock => mock
                .Get(application.EntityId))
                .Returns(application);

            const string auditEventTypeCode = AuditEventTypes.HardDeleteApprenticeshipApplication;

            // Act.
            _strategy.Audit(VacancyType.Apprenticeship, application.EntityId, auditEventTypeCode);

            // Assert.
            _auditRepository.Verify(mock => mock
                .Audit(application, auditEventTypeCode, application.EntityId, null), Times.Once);
        }

        [Test]
        public void ShouldAuditTraineeshipApplication()
        {
            // Arrange.
            var application = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .Create();

            _traineeshipApplicationReadRepository.Setup(mock => mock
                .Get(application.EntityId))
                .Returns(application);

            const string auditEventTypeCode = AuditEventTypes.HardDeleteTraineeshipApplication;

            // Act.
            _strategy.Audit(VacancyType.Traineeship, application.EntityId, auditEventTypeCode);

            // Assert.
            _auditRepository.Verify(mock => mock
                .Audit(application, auditEventTypeCode, application.EntityId, null), Times.Once);
        }

        [Test]
        public void ShouldThrowIfVacancyTypeUnknown()
        {
            // Act.
            Action action = () => _strategy.Audit(VacancyType.Unknown, Guid.NewGuid(), string.Empty);

            // Assert.
            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
