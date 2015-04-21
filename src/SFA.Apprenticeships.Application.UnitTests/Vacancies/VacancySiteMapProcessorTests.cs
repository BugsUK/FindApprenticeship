namespace SFA.Apprenticeships.Application.UnitTests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Vacancies;
    using Domain.Interfaces.Caching;
    using FluentAssertions;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;
    using Vacancy;
    using Web.Common.SiteMap;

    [TestFixture]
    public class VacancySiteMapProcessorTests
    {
        private const string ApprenticeshipVacancyIndexName = "apprenticeships";
        private const string TraineeshipVacancyIndexName = "traineeships";

        private Mock<ILogService> _mockLogger;
        private Mock<ICacheService> _mockCacheService;

        private Mock<IAllApprenticeshipVacanciesProvider> _mockApprenticeshipVacanciesProvider;
        private Mock<IAllTraineeshipVacanciesProvider> _mockTraineeshipVacanciesProvider;

        private List<SiteMapItem> _siteMapItems;
        private VacancySiteMapProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            // Common services.
            _mockLogger = new Mock<ILogService>();
            _mockCacheService = new Mock<ICacheService>();

            _mockCacheService.Setup(mock => mock
                .PutObject(It.IsAny<string>(), It.IsAny<object>(), CacheDuration.OneDay))
                .Callback((string cacheKey, object cacheObject, CacheDuration cacheDuration) =>
                    _siteMapItems.AddRange((SiteMapItem[])cacheObject)
                );

            _siteMapItems = new List<SiteMapItem>();

            // Providers.
            _mockApprenticeshipVacanciesProvider = new Mock<IAllApprenticeshipVacanciesProvider>();
            _mockTraineeshipVacanciesProvider = new Mock<IAllTraineeshipVacanciesProvider>();
        }

        [Test]
        public void ShouldCacheApprenticeshipVacancies()
        {
            // Arrange.
            _processor = new VacancySiteMapProcessor(
                _mockLogger.Object, _mockCacheService.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var ids = new[] { 1, 2, 3 };

            _mockApprenticeshipVacanciesProvider.Setup(mock => mock.
                GetAllVacancyIds(ApprenticeshipVacancyIndexName)).Returns(ids);

            // Act.
            _processor.CreateVacancySiteMap(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockCacheService.Verify(mock => mock.PutObject("SiteMap.Vacancies", It.IsAny<object>(), CacheDuration.OneDay), Times.Once);

            _siteMapItems.Should().HaveSameCount(ids);

            foreach (var id in ids)
            {
                var currentId = id;

                Assert.That(_siteMapItems.Any(each => each.Url.EndsWith(string.Format("apprenticeship/{0}", currentId))));
            }
        }

        [Test]
        public void ShouldCacheTraineeshipVacancies()
        {
            // Arrange.
            _processor = new VacancySiteMapProcessor(
                _mockLogger.Object, _mockCacheService.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var ids = new[] { 4, 5, 6, 42 };

            _mockTraineeshipVacanciesProvider.Setup(mock => mock.
                GetAllVacancyIds(TraineeshipVacancyIndexName)).Returns(ids);

            // Act.
            _processor.CreateVacancySiteMap(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockCacheService.Verify(mock => mock.PutObject("SiteMap.Vacancies", It.IsAny<object>(), CacheDuration.OneDay), Times.Once);

            _siteMapItems.Should().HaveSameCount(ids);

            foreach (var id in ids)
            {
                var currentId = id;

                Assert.That(_siteMapItems.Any(each => each.Url.EndsWith(string.Format("traineeship/{0}", currentId))));
            }
        }

        [Test]
        public void ShouldCacheApprenticeshipAndTraineeshipVacancies()
        {
            // Arrange.
            _processor = new VacancySiteMapProcessor(
                _mockLogger.Object, _mockCacheService.Object, _mockApprenticeshipVacanciesProvider.Object, _mockTraineeshipVacanciesProvider.Object);

            var apprenticeshipIds = new[] { 1, 2, 3 };
            var traineeshipIds = new[] { 4, 5, 6, 42 };

            _mockApprenticeshipVacanciesProvider.Setup(mock => mock.
                GetAllVacancyIds(ApprenticeshipVacancyIndexName)).Returns(apprenticeshipIds);

            _mockTraineeshipVacanciesProvider.Setup(mock => mock.
                GetAllVacancyIds(TraineeshipVacancyIndexName)).Returns(traineeshipIds);

            // Act.
            _processor.CreateVacancySiteMap(new CreateVacancySiteMapRequest
            {
                ApprenticeshipVacancyIndexName = ApprenticeshipVacancyIndexName,
                TraineeshipVacancyIndexName = TraineeshipVacancyIndexName
            });

            // Assert.
            _mockCacheService.Verify(mock => mock.PutObject("SiteMap.Vacancies", It.IsAny<object>(), CacheDuration.OneDay), Times.Once);

            _siteMapItems.Should().HaveCount(apprenticeshipIds.Length + traineeshipIds.Length);

            Assert.That(_siteMapItems.All(each => each != null));
            Assert.That(_siteMapItems.All(each => !string.IsNullOrWhiteSpace(each.Url)));
            Assert.That(_siteMapItems.All(each => each.ChangeFrequency == SiteMapChangeFrequency.Hourly));
            Assert.That(_siteMapItems.All(each => each.LastModified == DateTime.Today));
        }
    }
}