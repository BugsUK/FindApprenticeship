namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyStatusChange
{
    using System.Collections.Generic;
    using Application.Interfaces.Applications;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ArchiveVacancyTests
    {
        [TestCase(1, 0, 0, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 1, 0, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 1, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 0, 1, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 0, 0, 1, 0, 0, 1, 1, true)]
        [TestCase(0, 0, 0, 0, 0, 1, 0, 1, 1, true)]
        [TestCase(0, 0, 0, 0, 0, 0, 1, 1, 1, true)]
        public void ShouldReturnArchiveVacancyViewModelWithOutstandingApprenticeshipActions(int unknown, int saved, int draft, int expiredOrWithdrawn,
            int submitting, int submitted, int inProgress, int successful, int unsuccessful, bool shouldHaveOustandingActions)
        {
            //Arrange
            var vacancyId = -999999;
            var applicationSummaries = GetApplications<ApprenticeshipApplicationSummary>(unknown, saved, draft,
                expiredOrWithdrawn, submitting, submitted, inProgress, successful, unsuccessful);
            var mockTraineeshipService = new Mock<ITraineeshipApplicationService>();
            var mockVacancy = new Fixture().Build<Vacancy>().With(v => v.VacancyId, vacancyId)
                .With(v => v.VacancyType, VacancyType.Apprenticeship).Create();
            var mockVacancyPostingService = new Mock<IVacancyPostingService>();
            var mockVacancyRepo = new Mock<IVacancyReadRepository>();
            mockVacancyRepo.Setup(m => m.GetByReferenceNumber(It.IsAny<int>())).Returns(mockVacancy);
            var mockApprenticeshipService = new Mock<IApprenticeshipApplicationService>();
            mockApprenticeshipService.Setup(m => m.GetApplicationSummaries(It.IsAny<int>()))
                .Returns(applicationSummaries);
            var providerUnderTest = new VacancyStatusChangeProvider(mockApprenticeshipService.Object, mockTraineeshipService.Object, mockVacancyRepo.Object, mockVacancyPostingService.Object);

            //Act
            var result = providerUnderTest.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyId);

            //Assert
            result.HasOutstandingActions.Should().Be(shouldHaveOustandingActions);
        }

        [TestCase(1, 0, 0, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 1, 0, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 1, 0, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 0, 1, 0, 0, 0, 1, 1, false)]
        [TestCase(0, 0, 0, 0, 1, 0, 0, 1, 1, true)]
        [TestCase(0, 0, 0, 0, 0, 1, 0, 1, 1, true)]
        [TestCase(0, 0, 0, 0, 0, 0, 1, 1, 1, true)]
        public void ShouldReturnArchiveVacancyViewModelWithOutstandingTraineeshipActions(int unknown, int saved, int draft, int expiredOrWithdrawn,
            int submitting, int submitted, int inProgress, int successful, int unsuccessful, bool shouldHaveOustandingActions)
        {
            //Arrange
            var vacancyId = -999999;
            var applicationSummaries = GetApplications<TraineeshipApplicationSummary>(unknown, saved, draft,
                expiredOrWithdrawn, submitting, submitted, inProgress, successful, unsuccessful);
            var mockVacancy = new Fixture().Build<Vacancy>().With(v => v.VacancyId, vacancyId)
                .With(v => v.VacancyType, VacancyType.Traineeship).Create();
            var mockVacancyPostingService = new Mock<IVacancyPostingService>();
            var mockVacancyRepo = new Mock<IVacancyReadRepository>();
            mockVacancyRepo.Setup(m => m.GetByReferenceNumber(It.IsAny<int>())).Returns(mockVacancy);
            var mockApprenticeshipService = new Mock<IApprenticeshipApplicationService>();
            var mockTraineeshipService = new Mock<ITraineeshipApplicationService>();
            mockTraineeshipService.Setup(m => m.GetSubmittedApplicationSummaries(It.IsAny<int>()))
                .Returns(applicationSummaries);
            var providerUnderTest = new VacancyStatusChangeProvider(mockApprenticeshipService.Object, mockTraineeshipService.Object, mockVacancyRepo.Object, mockVacancyPostingService.Object);

            //Act
            var result = providerUnderTest.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyId);

            //Assert
            result.HasOutstandingActions.Should().Be(shouldHaveOustandingActions);
        }

        private IEnumerable<T> GetApplications<T>(int unknown, int saved, int draft,
            int expiredOrWithdrawn, int submitting, int submitted, int inProgress, int successful, int unsuccessful)
            where T : ApplicationSummary
        {
            var fixture = new Fixture();
            var unknownApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Unknown)
                    .CreateMany(unknown);

            var savedApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Saved)
                    .CreateMany(saved);

            var draftApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Draft)
                    .CreateMany(draft);

            var expiredOrWithdrawnApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.ExpiredOrWithdrawn)
                    .CreateMany(expiredOrWithdrawn);

            var submittingApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Submitting)
                    .CreateMany(submitting);

            var submittedApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Submitted)
                    .CreateMany(submitted);

            var inProgressApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.InProgress)
                    .CreateMany(inProgress);

            var successfulApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Successful)
                    .CreateMany(successful);

            var unsuccessfulApplications =
                fixture.Build<T>()
                    .With(x => x.Status, ApplicationStatuses.Unsuccessful)
                    .CreateMany(unsuccessful);

            var result = new List<T>();
            result.AddRange(unknownApplications);
            result.AddRange(savedApplications);
            result.AddRange(draftApplications);
            result.AddRange(expiredOrWithdrawnApplications);
            result.AddRange(submittedApplications);
            result.AddRange(inProgressApplications);
            result.AddRange(submittingApplications);
            result.AddRange(successfulApplications);
            result.AddRange(unsuccessfulApplications);

            return result;
        }
    }
}
