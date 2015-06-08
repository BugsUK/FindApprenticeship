namespace SFA.Apprenticeships.Application.UnitTests.Strategies
{
    using System;
    using Application.Applications.Strategies;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HardDeleteApplicationStrategyTests
    {
        private Mock<IApprenticeshipApplicationWriteRepository> _apprenticeshipApplicationWriteRepository;
        private Mock<ITraineeshipApplicationWriteRepository> _traineeshipApplicationWriteRepository;

        private HardDeleteApplicationStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _traineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();

            _strategy = new HardDeleteApplicationStrategy(
                _apprenticeshipApplicationWriteRepository.Object,
                _traineeshipApplicationWriteRepository.Object);
        }

        [Test]
        public void ShouldDeleteApprenticeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            // Act.
            _strategy.Delete(VacancyType.Apprenticeship, applicationId);

            // Assert.
            _apprenticeshipApplicationWriteRepository.Verify(mock => mock
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
            _traineeshipApplicationWriteRepository.Verify(mock => mock
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
