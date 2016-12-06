namespace SFA.Apprenticeships.Application.UnitTests.Applications.Housekeeping
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.Applications.Housekeeping;
    using Apprenticeships.Application.Applications.Strategies;
    using Apprenticeships.Application.Candidates.Configuration;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class DraftApplicationForExpiredVacancyHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<IHardDeleteApplicationStrategy> _mockHardDeleteApplicationStrategy;
        private Mock<IApplicationHousekeeper> _mockSuccessor;

        private HousekeepingConfiguration _housekeepingConfiguration;
        private DraftApplicationForExpiredVacancyHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();

            _housekeepingConfiguration = new Fixture()
                .Build<HousekeepingConfiguration>()
                .Create();

            _mockLogService = new Mock<ILogService>();

            _mockConfigurationService
                .Setup(mock => mock
                    .Get<HousekeepingConfiguration>())
                .Returns(_housekeepingConfiguration);

            _mockHardDeleteApplicationStrategy = new Mock<IHardDeleteApplicationStrategy>();
            _mockSuccessor = new Mock<IApplicationHousekeeper>();

            _housekeeper = new DraftApplicationForExpiredVacancyHousekeeper(
                _mockLogService.Object,
                _mockConfigurationService.Object,
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockHardDeleteApplicationStrategy.Object)
            {
                Successor = _mockSuccessor.Object
            };
        }

        [Test]
        public void ShouldGetApprenticeshipApplicationsForHousekeeping()
        {
            // Arrange.
            var applicationIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .GetDraftApplicationsForExpiredVacancies(It.IsAny<DateTime>()))
                .Returns(applicationIds);

            // Act.
            var requests = _housekeeper.GetHousekeepingRequests().ToList();

            // Assert.
            requests.Select(each => each.ApplicationId).ShouldAllBeEquivalentTo(applicationIds);
            Assert.That(requests.All(each => each.VacancyType == VacancyType.Apprenticeship));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(30)]
        public void ShouldHonourHousekeepingConfiguration(
            int hardDeleteDraftApplicationForExpiredVacancyAfterDays)
        {
            // Arrange.
            var applicationIds = new[]
            {
                Guid.NewGuid()
            };

            _housekeepingConfiguration.Application.HardDeleteDraftApplicationForExpiredVacancyAfterCycles = hardDeleteDraftApplicationForExpiredVacancyAfterDays;

            var expectedVacancyExpiryDate = GetVacancyExpiryDate(_housekeepingConfiguration);

            var actualVacancyExpiryDate = new DateTime();

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .GetDraftApplicationsForExpiredVacancies(It.IsAny<DateTime>()))
                .Returns(applicationIds)
                .Callback<DateTime>(vacancyExpiryDate => actualVacancyExpiryDate = vacancyExpiryDate);

            // Act.
            _housekeeper.GetHousekeepingRequests();

            // Assert.
            actualVacancyExpiryDate.Should().BeCloseTo(expectedVacancyExpiryDate, 500);
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void ShouldHandleDraftApprenticeshipApplicationForExpiredAndNonExpiredVacancy(
            int vacancyExpiredOffsetHours, bool shouldHandle)
        {
            // Arrange.
            var applicationId = Guid.NewGuid();
            var expiredVacancyDate = GetVacancyExpiryDate(_housekeepingConfiguration)
                .AddHours(-vacancyExpiredOffsetHours);

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, applicationId)
                .With(fixture => fixture.DateApplied, null)
                .With(fixture => fixture.Vacancy, new ApprenticeshipSummary
                {
                    ClosingDate = expiredVacancyDate
                })
                .Create();

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .Get(applicationId, false))
                .Returns(application);

            // Act.
            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Apprenticeship,
                ApplicationId = applicationId
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockHardDeleteApplicationStrategy.Verify(mock => mock
                .Delete(request.VacancyType, applicationId), Times.Exactly(shouldHandle ? 1 : 0));

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Exactly(shouldHandle ? 0 : 1));
        }

        
        [Test]
        public void ShouldIgnoreNonDraftApprenticeshipApplicationForExpiredVacancy()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();
            var expiredVacancyDate = GetVacancyExpiryDate(_housekeepingConfiguration);

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, applicationId)
                .With(fixture => fixture.DateApplied, DateTime.UtcNow)
                .With(fixture => fixture.Vacancy, new ApprenticeshipSummary
                {
                    ClosingDate = expiredVacancyDate
                })
                .Create();

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .Get(applicationId, false))
                .Returns(application);

            // Act.
            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Apprenticeship,
                ApplicationId = applicationId
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockHardDeleteApplicationStrategy.Verify(mock => mock
                .Delete(request.VacancyType, applicationId), Times.Never);

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Once);
        }

        [Test]
        public void ShouldHandleAlreadyDeletedApprenticeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .Get(applicationId, false))
                .Returns(default(ApprenticeshipApplicationDetail));

            // Act.
            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Apprenticeship,
                ApplicationId = applicationId
            };

            _housekeeper.Handle(request);

            // Assert.
            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Never);

        }

        [Test]
        public void ShouldThrowIfVacancyTypeUnknown()
        {
            // Arrange.
            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Unknown,
                ApplicationId = Guid.NewGuid()
            };

            // Act.
            Action action = () => _housekeeper.Handle(request);

            // Assert.
            action.ShouldThrow<InvalidOperationException>();
        }

        #region

        private static DateTime GetVacancyExpiryDate(HousekeepingConfiguration housekeepingConfiguration)
        {
            return DateTime.UtcNow.AddHours(
                -housekeepingConfiguration.HousekeepingCycleInHours *
                housekeepingConfiguration.Application.HardDeleteDraftApplicationForExpiredVacancyAfterCycles);
        }

        #endregion
    }
}
