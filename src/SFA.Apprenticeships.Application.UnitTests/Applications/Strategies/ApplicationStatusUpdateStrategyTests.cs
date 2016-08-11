namespace SFA.Apprenticeships.Application.UnitTests.Applications.Strategies
{
    using System;
    using Apprenticeships.Application.Applications.Entities;
    using Apprenticeships.Application.Applications.Strategies;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ApplicationStatusUpdateStrategyTests
    {
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<ITraineeshipApplicationWriteRepository> _mockTraineeshipApplicationWriteRepository;
        private Mock<IApplicationStatusAlertStrategy> _mockApplicationStatusAlertStrategy;
        private Mock<ILogService> _mockLogger;

        private ApplicationStatusUpdateStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockTraineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();
            _mockApplicationStatusAlertStrategy = new Mock<IApplicationStatusAlertStrategy>();
            _mockLogger = new Mock<ILogService>();

            _strategy = new ApplicationStatusUpdateStrategy(
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockApprenticeshipApplicationWriteRepository.Object,
                _mockTraineeshipApplicationWriteRepository.Object,
                _mockApplicationStatusAlertStrategy.Object,
                _mockLogger.Object);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSaveWhenApprenticeshipApplicationOrVacancyUpdated(bool updated)
        {
            // Arrange.
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            var apprenticeshipApplicationDetail = new ApprenticeshipApplicationDetail
            {   
                EntityId = Guid.NewGuid(),
                Vacancy = new ApprenticeshipSummary
                {
                    ClosingDate = tomorrow
                }
            };

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                ClosingDate = updated ? today : tomorrow
            };

            // Act.
            _strategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);

            // Assert.
            var times = updated ? 1 : 0;

            _mockApprenticeshipApplicationWriteRepository.Verify(mock => mock
                .Save(apprenticeshipApplicationDetail), Times.Exactly(times));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSendAlertWhenApprenticeshipApplicationOrVacancyUpdated(bool updated)
        {
            // Arrange.
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            var apprenticeshipApplicationDetail = new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Vacancy = new ApprenticeshipSummary
                {
                    ClosingDate = tomorrow
                }
            };

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                ClosingDate = updated ? today : tomorrow
            };

            // Act.
            _strategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);

            // Assert.
            var times = updated ? 1 : 0;

            _mockApplicationStatusAlertStrategy.Verify(mock => mock
                .Send(applicationStatusSummary), Times.Exactly(times));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSaveWhenTraineeshipApplicationOrVacancyUpdated(bool updated)
        {
            // Arrange.
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            var traineeshipApplicationDetail = new TraineeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Vacancy = new TraineeshipSummary
                {
                    ClosingDate = tomorrow
                }
            };

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                ClosingDate = updated ? today : tomorrow
            };

            // Act.
            _strategy.Update(traineeshipApplicationDetail, applicationStatusSummary);

            // Assert.
            var times = updated ? 1 : 0;

            _mockTraineeshipApplicationWriteRepository.Verify(mock => mock
                .Save(traineeshipApplicationDetail), Times.Exactly(times));
        }
    }
}
