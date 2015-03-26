namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.UnitTests.Vacancy
{
    using Apprenticeships.Application.Interfaces.Logging;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.Entities;
    using Moq;
    using NUnit.Framework;
    using Processes.Configuration;
    using Processes.Vacancies;
    using VacancyIndexer;

    [TestFixture]
    public class ApprenticeshipSummaryUpdateConsumerAsyncTests
    {
        private const int VacancyAboutToExpireNotificationHours = 96;
        private const string ASector = "FullName";
        private const string ASubcategory = "SubCategoryFullName";
        private const string AnotherSubcategory = "AnotherFramework";
        readonly Mock<IReferenceDataService> _referenceDataService = new Mock<IReferenceDataService>();
        readonly Mock<ILogService> _logService = new Mock<ILogService>();
        readonly Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _vacancyIndexer = new Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
        readonly Mock<IVacancySummaryProcessor> _vacancySummaryProcessor = new Mock<IVacancySummaryProcessor>();
        readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();

        [SetUp]
        public void SetUp()
        {
            _vacancyIndexer.ResetCalls();
            _logService.ResetCalls();
            _vacancySummaryProcessor.ResetCalls();
            _configurationService.Setup(x => x.Get<ProcessConfiguration>(ProcessConfiguration.ConfigurationName))
                .Returns(new ProcessConfiguration { VacancyAboutToExpireNotificationHours = VacancyAboutToExpireNotificationHours });
        }

        [Test]
        public void MismatchedSectorFramework()
        {
            var consumer = new ApprenticeshipSummaryUpdateConsumerAsync(_configurationService.Object, _vacancyIndexer.Object,
                _vacancySummaryProcessor.Object, _referenceDataService.Object, _logService.Object);

            SetupReferenceDataService(_referenceDataService);

            consumer.Consume(new ApprenticeshipSummaryUpdate
            {
                Sector = ASector,
                Framework = AnotherSubcategory
            }).Wait();

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
            _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Test]
        public void IndexVacancy()
        {
            var consumer = new ApprenticeshipSummaryUpdateConsumerAsync(_configurationService.Object, _vacancyIndexer.Object,
                _vacancySummaryProcessor.Object, _referenceDataService.Object, _logService.Object);

            SetupReferenceDataService(_referenceDataService);

            consumer.Consume(new ApprenticeshipSummaryUpdate
            {
                Sector = ASector,
                Framework = ASubcategory
            }).Wait();

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
            _vacancySummaryProcessor.Verify(vsp => vsp.QueueVacancyIfExpiring(It.IsAny<ApprenticeshipSummaryUpdate>(), It.Is<int>(i => i == VacancyAboutToExpireNotificationHours)));
            _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }



        private static void SetupReferenceDataService(Mock<IReferenceDataService> referenceDataService)
        {
            referenceDataService.Setup(rds => rds.GetCategories())
                .Returns(new[]
                {
                    new Category
                    {
                        CodeName = "01",
                        FullName = ASector,
                        SubCategories = new[] {new Category {CodeName = "02", FullName = ASubcategory}}
                    }
                });
        }
    }
}