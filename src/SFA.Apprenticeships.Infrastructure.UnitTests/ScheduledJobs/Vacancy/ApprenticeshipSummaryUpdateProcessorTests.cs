namespace SFA.Apprenticeships.Infrastructure.UnitTests.ScheduledJobs.Vacancy
{
    using System;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.Vacancies;
    using Infrastructure.VacancyIndexer;
    using Moq;
    using NUnit.Framework;
    using Apprenticeships.Application.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipSummaryUpdateProcessorTests
    {
        private const int VacancyAboutToExpireNotificationHours = 96;
        private const string ASector = "FullName";
        private const string ASubcategory = "SubCategoryFullName";
        private const string AnotherSubcategory = "AnotherFramework";
        readonly Mock<IReferenceDataService> _referenceDataService = new Mock<IReferenceDataService>();
        readonly Mock<ILogService> _logService = new Mock<ILogService>();
        readonly Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _vacancyIndexer = new Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
        readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        readonly Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();

        [SetUp]
        public void SetUp()
        {
            _vacancyIndexer.ResetCalls();
            _logService.ResetCalls();
            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    VacancyAboutToExpireNotificationHours = VacancyAboutToExpireNotificationHours,
                    StrictEtlValidation = true
                });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void MismatchedSectorFramework(bool strictEtlValidation)
        {
            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    VacancyAboutToExpireNotificationHours = VacancyAboutToExpireNotificationHours,
                    StrictEtlValidation = strictEtlValidation
                });

            var processor = new ApprenticeshipSummaryUpdateProcessor(
                _logService.Object,
                _serviceBus.Object,
                _configurationService.Object,
                _vacancyIndexer.Object,
                _referenceDataService.Object);

            SetupReferenceDataService(_referenceDataService);

            processor.Process(new ApprenticeshipSummaryUpdate
            {
                Category = ASector,
                SubCategory = AnotherSubcategory
            });

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);

            if (strictEtlValidation)
            {
                _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()));
            }
            else
            {
                _logService.Verify(ls => ls.Info(It.IsAny<string>(), It.IsAny<object[]>()));
            }
        }

        [Test]
        public void IndexVacancy()
        {

            var processor = new ApprenticeshipSummaryUpdateProcessor(
                _logService.Object,
                _serviceBus.Object,
                _configurationService.Object,
                _vacancyIndexer.Object,
                _referenceDataService.Object);

            SetupReferenceDataService(_referenceDataService);

            processor.Process(new ApprenticeshipSummaryUpdate
            {
                Category = ASector,
                SubCategory = ASubcategory
            });

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
            _serviceBus.Verify(mb => mb.PublishMessage(It.IsAny<VacancyAboutToExpire>()));
            _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        [Test]
        public void ShouldNotQueueTheVacancyIfTheVacancyIsNotAboutToExpire()
        {
            const int aVacancyId = 5;

            var processor = new ApprenticeshipSummaryUpdateProcessor(
                _logService.Object,
                _serviceBus.Object,
                _configurationService.Object,
                _vacancyIndexer.Object,
                _referenceDataService.Object);

            var vacancySummary = new ApprenticeshipSummaryUpdate
            {
                Id = aVacancyId,
                ClosingDate = DateTime.UtcNow.AddHours(VacancyAboutToExpireNotificationHours + 1)
            };

            processor.QueueVacancyIfExpiring(vacancySummary, VacancyAboutToExpireNotificationHours);

            _serviceBus.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)), Times.Never());
        }

        [Test]
        public void ShouldQueueTheVacancyIfTheVacancyIsAboutToExpire()
        {
            const int aVacancyId = 5;

            var processor = new ApprenticeshipSummaryUpdateProcessor(
                _logService.Object,
                _serviceBus.Object,
                _configurationService.Object,
                _vacancyIndexer.Object,
                _referenceDataService.Object);

            var vacancySummary = new ApprenticeshipSummaryUpdate
            {
                Id = aVacancyId,
                ClosingDate = DateTime.UtcNow.AddHours(VacancyAboutToExpireNotificationHours - 1)
            };

            processor.QueueVacancyIfExpiring(vacancySummary, VacancyAboutToExpireNotificationHours);

            _serviceBus.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)));
        }

        private static void SetupReferenceDataService(Mock<IReferenceDataService> referenceDataService)
        {
            referenceDataService.Setup(rds => rds.GetCategories())
                .Returns(new[]
                {
                    new Category(1, "01", ASector, CategoryType.SectorSubjectAreaTier1, new[] {new Category(2, "02", ASubcategory, CategoryType.Framework)})
                });
        }
    }
}