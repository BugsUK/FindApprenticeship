namespace SFA.Apprenticeships.Application.UnitTests.Applications.Housekeeping
{
    using System;
    using System.Linq;
    using Apprenticeships.Application.Applications.Housekeeping;
    using Apprenticeships.Application.Applications.Strategies;
    using Apprenticeships.Application.Candidates.Configuration;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class SubmittedApplicationHousekeeperTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<IApprenticeshipApplicationReadRepository> _mockApprenticeshipApplicationReadRepository;
        private Mock<ITraineeshipApplicationReadRepository> _mockTraineeshipApplicationReadRepository;
        private Mock<IHardDeleteApplicationStrategy> _mockHardDeleteApplicationStrategy;
        private Mock<IApplicationHousekeeper> _mockSuccessor;

        private HousekeepingConfiguration _housekeepingConfiguration;
        private SubmittedApplicationHousekeeper _housekeeper;

        [SetUp]
        public void SetUp()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockApprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            _mockTraineeshipApplicationReadRepository = new Mock<ITraineeshipApplicationReadRepository>();

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

            _housekeeper = new SubmittedApplicationHousekeeper(
                _mockLogService.Object,
                _mockConfigurationService.Object,
                _mockApprenticeshipApplicationReadRepository.Object,
                _mockTraineeshipApplicationReadRepository.Object,
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
                    .GetApplicationsSubmittedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(applicationIds);

            // Act.
            var requests = _housekeeper.GetHousekeepingRequests().ToList();

            // Assert.
            requests.Select(each => each.ApplicationId).ShouldAllBeEquivalentTo(applicationIds);
            Assert.That(requests.All(each => each.VacancyType == VacancyType.Apprenticeship));
        }

        [Test]
        public void ShouldGetTraineeshipApplicationsForHousekeeping()
        {
            // Arrange.
            var applicationIds = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _mockTraineeshipApplicationReadRepository
                .Setup(mock => mock
                    .GetApplicationsSubmittedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(applicationIds);

            // Act.
            var requests = _housekeeper.GetHousekeepingRequests().ToList();

            // Assert.
            requests.Select(each => each.ApplicationId).ShouldAllBeEquivalentTo(applicationIds);
            Assert.That(requests.All(each => each.VacancyType == VacancyType.Traineeship));
        }

        [TestCase(1)]
        [TestCase(7)]
        [TestCase(30)]
        public void ShouldHonourHousekeepingConfiguration(
            int hardDeleteSubmittedApplicationAfterCycles)
        {
            // Arrange.
            var applicationIds = new[]
            {
                Guid.NewGuid()
            };

            _housekeepingConfiguration.Application.HardDeleteSubmittedApplicationAfterCycles = hardDeleteSubmittedApplicationAfterCycles;

            var expectedDateApplied = GetHousekeepingDate(_housekeepingConfiguration);
            var actualDateApplied = new DateTime();

            _mockApprenticeshipApplicationReadRepository
                .Setup(mock => mock
                    .GetApplicationsSubmittedOnOrBefore(It.IsAny<DateTime>()))
                .Returns(applicationIds)
                .Callback<DateTime>(dateApplied => actualDateApplied = dateApplied);

            // Act.
            _housekeeper.GetHousekeepingRequests();

            // Assert.
            actualDateApplied.Should().BeCloseTo(expectedDateApplied, 500);
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void ShouldHandleSubmitedApprenticeshipApplicationOnOrBeforeDateApplied(
            int dateAppliedOffsetHours, bool shouldHandle)
        {
            // Arrange.
            var applicationId = Guid.NewGuid();
            var dateApplied = GetHousekeepingDate(_housekeepingConfiguration)
                .AddHours(-dateAppliedOffsetHours);

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, applicationId)
                .With(fixture => fixture.DateApplied, dateApplied)
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
        public void ShouldHandleUnsubmitedApprenticeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, applicationId)
                .With(fixture => fixture.DateApplied, default(DateTime?))
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
                .Delete(request.VacancyType, applicationId), Times.Exactly(0));

            _mockSuccessor.Verify(mock => mock
                .Handle(request), Times.Exactly(1));
        }

        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void ShouldHandleSubmitedTraineeshipApplicationOnOrBeforeDateApplied(
            int dateAppliedOffsetHours, bool shouldHandle)
        {
            // Arrange.
            var applicationId = Guid.NewGuid();
            var dateApplied = GetHousekeepingDate(_housekeepingConfiguration)
                .AddHours(-dateAppliedOffsetHours);

            var application = new Fixture()
                .Build<TraineeshipApplicationDetail>()
                .With(fixture => fixture.EntityId, applicationId)
                .With(fixture => fixture.DateApplied, dateApplied)
                .Create();

            _mockTraineeshipApplicationReadRepository
                .Setup(mock => mock
                    .Get(applicationId, false))
                .Returns(application);

            // Act.
            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Traineeship,
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
        public void ShouldHandleAlreadyDeletedTraineeshipApplication()
        {
            // Arrange.
            var applicationId = Guid.NewGuid();

            _mockTraineeshipApplicationReadRepository
                .Setup(mock => mock
                    .Get(applicationId, false))
                .Returns(default(TraineeshipApplicationDetail));

            var request = new ApplicationHousekeepingRequest
            {
                VacancyType = VacancyType.Traineeship,
                ApplicationId = applicationId
            };

            // Act.
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

        private static DateTime GetHousekeepingDate(HousekeepingConfiguration housekeepingConfiguration)
        {
            return DateTime.UtcNow.AddHours(
                -housekeepingConfiguration.HousekeepingCycleInHours *
                housekeepingConfiguration.Application.HardDeleteSubmittedApplicationAfterCycles);
        }

        #endregion
    }
}