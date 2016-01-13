namespace SFA.Apprenticeships.Application.UnitTests.Strategies
{
    using System;
    using Application.Applications.Strategies;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HardDeleteApplicationStrategyTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<ITraineeshipApplicationWriteRepository> _mockTraineeshipApplicationWriteRepository;

        private HardDeleteApplicationStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockTraineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();

            _strategy = new HardDeleteApplicationStrategy(
                _mockLogService.Object,
                _mockApprenticeshipApplicationWriteRepository.Object,
                _mockTraineeshipApplicationWriteRepository.Object);
        }

        [Test]
        public void ShouldDeleteApprenticeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            // Act.
            _strategy.Delete(VacancyType.Apprenticeship, applicationId);

            // Assert.
            _mockApprenticeshipApplicationWriteRepository.Verify(mock => mock
                .Delete(applicationId), Times.Once);
        }

        [Test]
        public void ShouldDeleteTraineeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            // Act.
            _strategy.Delete(VacancyType.Traineeship, applicationId);

            // Assert.
            _mockTraineeshipApplicationWriteRepository.Verify(mock => mock
                .Delete(applicationId), Times.Once);
        }

        [Test]
        public void ShouldThrowIfVacancyTypeUnknown()
        {
            // Act.
            Action action = () => _strategy.Delete(VacancyType.Unknown, Guid.NewGuid());

            // Assert.
            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
