namespace SFA.Apprenticeships.Application.UnitTests.Application
{
    using Apprenticeships.Application.Application;
    using Apprenticeships.Application.Application.Entities;
    using Apprenticeships.Application.Application.Strategies;
    using Apprenticeships.Application.Application.Strategies.Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using Domain.Interfaces.Messaging;

    [TestFixture]
    public class ApprenticeshipApplicationServiceTests
    {
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IApprenticeshipApplicationWriteRepository> _mockApprenticeshipApplicationWriteRepository;
        private Mock<IApprenticeshipApplicationStatsRepository> _mockApprenticeshipApplicationStatsRepository;
        private Mock<IReferenceNumberRepository> _mockReferenceNumberRepository;
        private Mock<IGetApplicationForReviewStrategy> _mockGetApplicationForReviewStrategy;
        private Mock<IUpdateApplicationNotesStrategy> _mockUpdateApplicationNotesStrategy;
        private Mock<IApplicationStatusUpdateStrategy> _mockApplicationStatusUpdateStrategy;

        private ApprenticeshipApplicationService _apprenticeshipApplicationService;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockApprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            _mockApprenticeshipApplicationStatsRepository = new Mock<IApprenticeshipApplicationStatsRepository>();
            _mockReferenceNumberRepository = new Mock<IReferenceNumberRepository>();
            _mockGetApplicationForReviewStrategy = new Mock<IGetApplicationForReviewStrategy>();
            _mockUpdateApplicationNotesStrategy = new Mock<IUpdateApplicationNotesStrategy>();
            _mockApplicationStatusUpdateStrategy = new Mock<IApplicationStatusUpdateStrategy>();
            var setApplicationStatusStrategy = new SetApplicationStatusStrategy(_mockApprenticeshipApplicationReadRepository.Object, _mockApprenticeshipApplicationWriteRepository.Object, _mockReferenceNumberRepository.Object, _mockApplicationStatusUpdateStrategy.Object, new Mock<IServiceBus>().Object);

            _apprenticeshipApplicationService = new ApprenticeshipApplicationService(
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockApprenticeshipApplicationStatsRepository.Object,
                _mockGetApplicationForReviewStrategy.Object,
                _mockUpdateApplicationNotesStrategy.Object,
                setApplicationStatusStrategy);
        }

        [Test]
        public void GetCountsForVacancyIds()
        {
            // TODO: This test doesn't really have any value, but it has been kept working anyway

            //Arrange

            var vacancy = new ApprenticeshipSummary
            {
                ClosingDate = DateTime.Today.AddDays(90),
                Id = 1
            };

            var expectedCounts = new Mock<IApplicationCounts>();
            expectedCounts.Setup(mock => mock.AllApplications).Returns(2);
            expectedCounts.Setup(mock => mock.NewApplications).Returns(1);

            var expected = new Dictionary<int, IApplicationCounts> {
                { vacancy.Id, expectedCounts.Object }
            };

            var vacancies = new int[] { vacancy.Id };

            _mockApprenticeshipApplicationStatsRepository.Setup(mock =>
                mock.GetCountsForVacancyIds(vacancies)).Returns(expected);

            //Act            
            var response = _apprenticeshipApplicationService.GetCountsForVacancyIds(vacancies);

            //Assert            
            Assert.AreEqual(response, expected);
        }

        [Test]
        public void GetCountsForVacancyIds_GetApplicationCount()
        {
            // TODO: This test doesn't really have any value, but it has been kept working anyway

            //Arrange

            var vacancy = new ApprenticeshipSummary
            {
                ClosingDate = DateTime.Today.AddDays(90),
                Id = 1
            };

            var expectedCounts = new Mock<IApplicationCounts>();
            expectedCounts.Setup(mock => mock.AllApplications).Returns(2);
            expectedCounts.Setup(mock => mock.NewApplications).Returns(1);

            var expected = new Dictionary<int, IApplicationCounts> {
                { vacancy.Id, expectedCounts.Object }
            };

            var vacancies = new int[] { vacancy.Id };

            _mockApprenticeshipApplicationStatsRepository.Setup(mock =>
                mock.GetCountsForVacancyIds(vacancies)).Returns(expected);

            //Act            
            var response = _apprenticeshipApplicationService.GetApplicationCount(vacancy.Id);

            //Assert            
            Assert.AreEqual(response, expected[vacancy.Id].AllApplications);
        }

        [TestCase(ApplicationStatuses.Successful)]
        [TestCase(ApplicationStatuses.Unsuccessful)]
        public void ShouldSetSuccessfulOutcome(ApplicationStatuses applicationStatus)
        {
            // Arrange.
            var applicationId = Guid.NewGuid();
            const int nextLegacyApplicationId = 2;

            var apprenticeshipApplicationDetail = new ApprenticeshipApplicationDetail
            {
                VacancyStatus = VacancyStatuses.Live,
                Vacancy = new ApprenticeshipSummary
                {
                    ClosingDate = DateTime.Today.AddDays(90)
                }
            };

            _mockApprenticeshipApplicationReadRepository.Setup(mock =>
                mock.Get(applicationId)).Returns(apprenticeshipApplicationDetail);

            var actualApplicationStatusSummary = default(ApplicationStatusSummary);

            _mockReferenceNumberRepository.Setup(mock =>
                mock.GetNextLegacyApplicationId())
                .Returns(nextLegacyApplicationId);

            _mockApplicationStatusUpdateStrategy.Setup(mock =>
                mock.Update(apprenticeshipApplicationDetail, It.IsAny<ApplicationStatusSummary>()))
                .Callback<ApprenticeshipApplicationDetail, ApplicationStatusSummary>(
                    (aad, ass) =>
                    {
                        actualApplicationStatusSummary = ass;
                    });

            // Act.
            switch (applicationStatus)
            {
                case ApplicationStatuses.Successful:
                    _apprenticeshipApplicationService.SetSuccessfulDecision(applicationId);
                    break;

                case ApplicationStatuses.Unsuccessful:
                    _apprenticeshipApplicationService.SetUnsuccessfulDecision(applicationId);
                    break;
            }

            // Assert.
            _mockApplicationStatusUpdateStrategy.Verify(mock => mock.Update(apprenticeshipApplicationDetail, It.IsAny<ApplicationStatusSummary>()), Times.Once);

            actualApplicationStatusSummary.Should().NotBeNull();

            actualApplicationStatusSummary.ShouldBeEquivalentTo(new ApplicationStatusSummary
            {
                ApplicationId = Guid.Empty,
                ApplicationStatus = applicationStatus,
                LegacyApplicationId = nextLegacyApplicationId,
                LegacyCandidateId = 0,
                LegacyVacancyId = 0,
                VacancyStatus = apprenticeshipApplicationDetail.VacancyStatus,
                ClosingDate = apprenticeshipApplicationDetail.Vacancy.ClosingDate,
                UpdateSource = ApplicationStatusSummary.Source.Raa
            });
        }
    }
}
