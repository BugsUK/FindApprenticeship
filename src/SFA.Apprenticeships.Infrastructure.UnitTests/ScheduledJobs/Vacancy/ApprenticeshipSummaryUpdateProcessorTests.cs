namespace SFA.Apprenticeships.Infrastructure.UnitTests.ScheduledJobs.Vacancy
{
    using System;
    using Apprenticeships.Application.Interfaces.Logging;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.Vacancies;
    using Moq;
    using NUnit.Framework;
    using VacancyIndexer;

    [TestFixture]
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
        readonly Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();

        [SetUp]
        public void SetUp()
        {
            _vacancyIndexer.ResetCalls();
            _logService.ResetCalls();
            _configurationService.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration { VacancyAboutToExpireNotificationHours = VacancyAboutToExpireNotificationHours });
        }

        [Test]
        public void MismatchedSectorFramework()
        {
            var processor = new ApprenticeshipSummaryUpdateProcessor(_configurationService.Object, _vacancyIndexer.Object, _referenceDataService.Object, _messageBus.Object, _logService.Object);

            SetupReferenceDataService(_referenceDataService);

            processor.Process(new ApprenticeshipSummaryUpdate
            {
                Sector = ASector,
                Framework = AnotherSubcategory
            });

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
            _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Test]
        public void IndexVacancy()
        {
            var processor = new ApprenticeshipSummaryUpdateProcessor(_configurationService.Object, _vacancyIndexer.Object, _referenceDataService.Object, _messageBus.Object, _logService.Object);

            SetupReferenceDataService(_referenceDataService);

            processor.Process(new ApprenticeshipSummaryUpdate
            {
                Sector = ASector,
                Framework = ASubcategory
            });

            _vacancyIndexer.Verify(vi => vi.Index(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
            _messageBus.Verify(mb => mb.PublishMessage(It.IsAny<VacancyAboutToExpire>()));
            _logService.Verify(ls => ls.Warn(It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
        }

        [Test]
        public void ShouldNotQueueTheVacancyIfTheVacancyIsNotAboutToExpire()
        {
            const int aVacancyId = 5;
            var processor = new ApprenticeshipSummaryUpdateProcessor(_configurationService.Object, _vacancyIndexer.Object, _referenceDataService.Object, _messageBus.Object, _logService.Object);

            var vacancySummary = new ApprenticeshipSummaryUpdate
            {
                Id = aVacancyId,
                ClosingDate = DateTime.Now.AddHours(VacancyAboutToExpireNotificationHours + 1)
            };

            processor.QueueVacancyIfExpiring(vacancySummary, VacancyAboutToExpireNotificationHours);

            _messageBus.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)), Times.Never());
        }

        [Test]
        public void ShouldQueueTheVacancyIfTheVacancyIsAboutToExpire()
        {
            const int aVacancyId = 5;
            var processor = new ApprenticeshipSummaryUpdateProcessor(_configurationService.Object, _vacancyIndexer.Object, _referenceDataService.Object, _messageBus.Object, _logService.Object);

            var vacancySummary = new ApprenticeshipSummaryUpdate
            {
                Id = aVacancyId,
                ClosingDate = DateTime.Now.AddHours(VacancyAboutToExpireNotificationHours - 1)
            };

            processor.QueueVacancyIfExpiring(vacancySummary, VacancyAboutToExpireNotificationHours);

            _messageBus.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)));
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