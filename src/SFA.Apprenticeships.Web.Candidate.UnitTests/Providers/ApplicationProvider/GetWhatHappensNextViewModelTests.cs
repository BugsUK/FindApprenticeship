namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Common.Configuration;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetWhatHappensNextViewModelTests
    {
        const int ValidVacancyId = 1;

        [Test]
        public void GivenException_ThenCreateOrRetrieveApplicationFailedIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Throws<Exception>();

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService).Build().GetWhatHappensNextViewModel(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.CreateOrRetrieveApplicationFailed);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenNoApplicationFound_ThenApplicationNotFoundIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns((ApprenticeshipApplicationDetail) null);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build);

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService).Build().GetWhatHappensNextViewModel(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenNoCandidateFound_ThenApplicationNotFoundIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(candidateId, ValidVacancyId).Build);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns((Candidate) null);

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder().With(candidateService).Build().GetWhatHappensNextViewModel(candidateId, ValidVacancyId);
            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApplicationNotFound);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenPatchWithVacancyDetailHasError_ThenMessageIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(candidateId, ValidVacancyId).Build);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build);

            apprenticeshipVacancyProvider.Setup(cs => cs.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns((ApprenticeshipVacancyDetailViewModel)null);
            
            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).With(apprenticeshipVacancyProvider).Build()
                .GetWhatHappensNextViewModel(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeTrue();
            returnedViewModel.ViewModelMessage.Should().NotBeNullOrEmpty();
            returnedViewModel.ViewModelMessage.Should().Be(MyApplicationsPageMessages.ApprenticeshipNoLongerAvailable);
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
        }

        [Test]
        public void GivenGetWhatHappensNextViewModelIsSuccessful_ThenViewModelIsReturned()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(candidateId, ValidVacancyId).Build);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build);
            apprenticeshipVacancyProvider.Setup(cs => cs.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new ApprenticeshipVacancyDetailViewModelBuilder().Build());

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<WebConfiguration>())
                .Returns(new WebConfiguration() {FeedbackUrl = "http://feedback"});

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).With(apprenticeshipVacancyProvider).With(configurationService).Build()
                .GetWhatHappensNextViewModel(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.Status.Should().Be(ApplicationStatuses.Unknown);
            returnedViewModel.FeedbackUrl.Should().Be("http://feedback");
        }

        [Test]
        public void GivenVacancyIsExpired_ThenViewModelStatusIsExpired()
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            var apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            candidateService.Setup(cs => cs.GetApplication(candidateId, ValidVacancyId)).Returns(new ApprenticeshipApplicationDetailBuilder(candidateId, ValidVacancyId).Build);
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build);
            apprenticeshipVacancyProvider.Setup(cs => cs.GetVacancyDetailViewModel(candidateId, ValidVacancyId)).Returns(new ApprenticeshipVacancyDetailViewModelBuilder().WithVacancyStatus(VacancyStatuses.Expired).Build());

            var returnedViewModel = new ApprenticeshipApplicationProviderBuilder()
                .With(candidateService).With(apprenticeshipVacancyProvider).Build()
                .GetWhatHappensNextViewModel(candidateId, ValidVacancyId);

            returnedViewModel.HasError().Should().BeFalse();
            returnedViewModel.ViewModelMessage.Should().BeNullOrEmpty();
            returnedViewModel.VacancyStatus.Should().Be(VacancyStatuses.Expired);
        }
    }
}