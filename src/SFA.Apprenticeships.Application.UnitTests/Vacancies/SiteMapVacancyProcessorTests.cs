namespace SFA.Apprenticeships.Application.UnitTests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancies.Entities.SiteMap;
    using Apprenticeships.Application.Vacancy;
    using Apprenticeships.Application.Vacancy.SiteMap;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMapVacancyProcessorTests
    {
        private const string ApprenticeshipVacancyIndexName = "apprenticeships";
        private const string TraineeshipVacancyIndexName = "traineeships";

        private Mock<ILogService> _mockLogger;
        private Mock<ISiteMapVacancyProvider> _mockSiteMapVacancyProvider;

        private Mock<IAllApprenticeshipVacanciesProvider> _mockApprenticeshipVacanciesProvider;
        private Mock<IAllTraineeshipVacanciesProvider> _mockTraineeshipVacanciesProvider;

        private List<SiteMapVacancy> _siteMapVacancies;
        private SiteMapVacancyProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            // Common services.
            _mockLogger = new Mock<ILogService>();
            _mockSiteMapVacancyProvider = new Mock<ISiteMapVacancyProvider>();

            _mockSiteMapVacancyProvider.Setup(mock => mock
                .SetVacancies(It.IsAny<IEnumerable<SiteMapVacancy>>()))
                .Callback((IEnumerable<SiteMapVacancy> siteMapVacancies) =>
                    _siteMapVacancies = siteMapVacancies.ToList());

            _siteMapVacancies = null;

            // Providers.
            _mockApprenticeshipVacanciesProvider = new Mock<IAllApprenticeshipVacanciesProvider>();
            _mockTraineeshipVacanciesProvider = new Mock<IAllTraineeshipVacanciesProvider>();
        }

        [Test]
        public void ShouldCacheApprenticeshipVacancies()
        {
            // Arrange.
            _processor = new SiteMapVacancyProcessor(
                _mockLogger.Object, _mockSiteMapVacancyProvider.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var vacancyIds = new[] { 1, 2, 3 };

            _mockApprenticeshipVacanciesProvider.Setup(mock => mock
                .GetAllVacancyIds(ApprenticeshipVacancyIndexName)).Returns(vacancyIds);

            // Act.
            _processor.Process(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockSiteMapVacancyProvider.Verify(mock => mock
                .SetVacancies(It.IsAny<IEnumerable<SiteMapVacancy>>()), Times.Once);

            _siteMapVacancies.Should().NotBeNull();
            _siteMapVacancies.Should().HaveSameCount(vacancyIds);

            foreach (var vacancyId in vacancyIds)
            {
                Assert.That(_siteMapVacancies.Any(each => each.VacancyId == vacancyId));
            }

            Assert.That(_siteMapVacancies.All(each => each.VacancyType == VacancyType.Apprenticeship));
            Assert.That(_siteMapVacancies.All(each => each.LastModifiedDate != DateTime.MinValue));
        }

        [Test]
        public void ShouldCacheTraineeshipVacancies()
        {
            // Arrange.
            _processor = new SiteMapVacancyProcessor(
                _mockLogger.Object, _mockSiteMapVacancyProvider.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var vacancyIds = new[] { 4, 5, 6, 42 };

            _mockTraineeshipVacanciesProvider.Setup(mock => mock
                .GetAllVacancyIds(TraineeshipVacancyIndexName)).Returns(vacancyIds);

            // Act.
            _processor.Process(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockSiteMapVacancyProvider.Verify(mock => mock
                .SetVacancies(It.IsAny<IEnumerable<SiteMapVacancy>>()), Times.Once);

            _siteMapVacancies.Should().NotBeNull();
            _siteMapVacancies.Should().HaveSameCount(vacancyIds);

            foreach (var vacancyId in vacancyIds)
            {
                Assert.That(_siteMapVacancies.Any(each => each.VacancyId == vacancyId));
            }

            Assert.That(_siteMapVacancies.All(each => each.VacancyType == VacancyType.Traineeship));
            Assert.That(_siteMapVacancies.All(each => each.LastModifiedDate != DateTime.MinValue));
        }

        [Test]
        public void ShouldCacheApprenticeshipAndTraineeshipVacancies()
        {
            // Arrange.
            _processor = new SiteMapVacancyProcessor(
                _mockLogger.Object, _mockSiteMapVacancyProvider.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var apprenticeshipVacancyIds = new[] { 1, 2, 3 };
            var traineeshipVacancyIds = new[] { 4, 5, 6, 42 };

            _mockApprenticeshipVacanciesProvider.Setup(mock => mock
                .GetAllVacancyIds(ApprenticeshipVacancyIndexName)).Returns(apprenticeshipVacancyIds);

            _mockTraineeshipVacanciesProvider.Setup(mock => mock
                .GetAllVacancyIds(TraineeshipVacancyIndexName)).Returns(traineeshipVacancyIds);

            // Act.
            _processor.Process(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockSiteMapVacancyProvider.Verify(mock => mock
                .SetVacancies(It.IsAny<IEnumerable<SiteMapVacancy>>()), Times.Once);

            _siteMapVacancies.Should().NotBeNull();
            _siteMapVacancies.Should().HaveCount(apprenticeshipVacancyIds.Length + traineeshipVacancyIds.Length);

            Assert.That(_siteMapVacancies.All(each => each != null));
            Assert.That(_siteMapVacancies.All(each => each.VacancyId != 0));

            _siteMapVacancies
                .Count(each => each.VacancyType == VacancyType.Apprenticeship)
                .Should()
                .Be(apprenticeshipVacancyIds.Length);

            _siteMapVacancies
                .Count(each => each.VacancyType == VacancyType.Traineeship)
                .Should()
                .Be(traineeshipVacancyIds.Length);
        }
    }
}