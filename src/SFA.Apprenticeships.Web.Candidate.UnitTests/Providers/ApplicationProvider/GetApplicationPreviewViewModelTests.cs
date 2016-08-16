namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class GetApplicationPreviewViewModelTests
    {
        private const int ValidVacancyId = 42;

        [Test]
        public void ShouldGetApplicationPreviewViewModel()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();

            var vacancy = new Fixture()
                .Build<ApprenticeshipVacancyDetailViewModel>()
                .With(fixture => fixture.VacancyStatus, VacancyStatuses.Live)
                .With(fixture => fixture.ViewModelMessage, null)
                .Create();

            apprenticeshipVacancyProvider
                .Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId))
                .Returns(vacancy);

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.Status, ApplicationStatuses.Draft)
                .Create();

            var candidate = new Fixture()
                .Build<Candidate>()
                .Create();

            candidateService
                .Setup(cs => cs.GetApplication(candidateId, ValidVacancyId))
                .Returns(application);

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(candidate);

            var provider = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService)
                .With(apprenticeshipVacancyProvider)
                .Build();

            // Act.
            var applicationViewModel = provider.GetApplicationViewModel(candidateId, ValidVacancyId);
            var applicationPreviewViewModel = provider.GetApplicationPreviewViewModel(candidateId, ValidVacancyId);

            // Assert.
            applicationPreviewViewModel.Should().NotBeNull();

            applicationPreviewViewModel.DateApplied.Should().Be(applicationViewModel.DateApplied);
            applicationPreviewViewModel.DateUpdated.Should().Be(applicationViewModel.DateUpdated);

            applicationPreviewViewModel.VacancyId.Should().Be(applicationViewModel.VacancyId);
            applicationPreviewViewModel.VacancyDetail.Should().BeSameAs(applicationViewModel.VacancyDetail);

            applicationPreviewViewModel.Status.Should().Be(applicationViewModel.Status);
            applicationPreviewViewModel.AcceptSubmit.Should().BeFalse();

            applicationPreviewViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            applicationPreviewViewModel.HasError().Should().BeFalse();
        }

        [Test]
        public void ShouldHaveViewModelErrorMessage()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();

            var vacancy = new Fixture()
                .Build<ApprenticeshipVacancyDetailViewModel>()
                .With(fixture => fixture.VacancyStatus, VacancyStatuses.Live)
                .With(fixture => fixture.ViewModelMessage, "Something wonderful happened")
                .Create();

            apprenticeshipVacancyProvider
                .Setup(p => p.GetVacancyDetailViewModel(candidateId, ValidVacancyId))
                .Returns(vacancy);

            var application = new Fixture()
                .Build<ApprenticeshipApplicationDetail>()
                .With(fixture => fixture.Status, ApplicationStatuses.Draft)
                .Create();

            candidateService
                .Setup(cs => cs.GetApplication(candidateId, ValidVacancyId))
                .Returns(application);

            var provider = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService)
                .With(apprenticeshipVacancyProvider)
                .Build();

            // Act.
            var applicationPreviewViewModel = provider.GetApplicationPreviewViewModel(candidateId, ValidVacancyId);

            // Assert.
            applicationPreviewViewModel.Should().NotBeNull();

            applicationPreviewViewModel.ViewModelMessage.Should().Be(vacancy.ViewModelMessage);
            applicationPreviewViewModel.HasError().Should().BeTrue();
        }
    }
}
