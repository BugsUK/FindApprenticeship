namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using System;
    using Builders;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PatchApplicationViewModelTests
    {
        [Test]
        public void GivenNullViewModels_ThenExceptionIsThrown()
        {
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, null);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenNullCandidateViewModel_ThenExceptionIsThrown()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = null
            };
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, viewModel);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenNullAboutYouViewModel_ThenExceptionIsThrown()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    AboutYou = null
                }
            };
            Action patchApplicationViewModelAction = () => new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), null, viewModel);
            patchApplicationViewModelAction.ShouldThrow<Exception>();
        }

        [Test]
        public void GivenCandidateDoesNotRequireSupportForInterview_ThenSupportMessageIsBlanked()
        {
            var savedModel = new ApprenticeshipApplicationViewModelBuilder().Build();
            var submittedModel = new ApprenticeshipApplicationViewModelBuilder()
                .IsJavascript(true)
                .DoesNotRequireSupportForInterview()
                .CanBeSupportedAtInterviewBy("Should be blanked")
                .Build();

            var patchedViewModel = new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
            patchedViewModel.Candidate.MonitoringInformation.AnythingWeCanDoToSupportYourInterview.Should().BeNullOrEmpty();
        }

        [Test]
        public void GivenRequiresSupportForInterview_ThenSupportMessageIsRetained()
        {
            var savedModel = new ApprenticeshipApplicationViewModelBuilder().Build();
            const string anythingWeCanDoToSupportYourInterview = "Should be retained";
            var submittedModel = new ApprenticeshipApplicationViewModelBuilder()
                .RequiresSupportForInterview()
                .CanBeSupportedAtInterviewBy(anythingWeCanDoToSupportYourInterview)
                .Build();

            var patchedViewModel = new ApprenticeshipApplicationProviderBuilder().Build().PatchApplicationViewModel(Guid.NewGuid(), savedModel, submittedModel);
            patchedViewModel.Candidate.MonitoringInformation.AnythingWeCanDoToSupportYourInterview.Should().Be(anythingWeCanDoToSupportYourInterview);
        }

        [Test]
        public void ShouldPatchWorkExperienceChanges()
        {
            var candidateId = Guid.NewGuid();
            var apprenticeshipApplicationProvider = new ApprenticeshipApplicationProviderBuilder().Build();
            var savedViewModel = new ApprenticeshipApplicationViewModelBuilder().Build();

            var workExperience = new[]
            {
                new WorkExperienceViewModelBuilder()
                    .WithDescription("Work")
                    .WithEmployer("Employer")
                    .Build()
            };

            var newViewModel = new ApprenticeshipApplicationViewModelBuilder().WithWorkExperience(workExperience).Build();
            var patchedViewModel = apprenticeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedViewModel, newViewModel);

            patchedViewModel.Should().NotBeNull();
            patchedViewModel.ViewModelMessage.Should().BeNull();
            patchedViewModel.HasError().Should().BeFalse();
            patchedViewModel.Candidate.HasWorkExperience.Should().BeTrue();
            patchedViewModel.Candidate.WorkExperience.Should().Equal(workExperience);
        }

        [Test]
        public void ShouldPatchTrainingCourseChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new ApprenticeshipApplicationProviderBuilder().Build();
            var savedTraineeshipViewModel = new ApprenticeshipApplicationViewModelBuilder().Build();

            var trainingCourses = new[]
            {
                new TrainingCourseViewModelBuilder()
                    .WithProvider("Provider")
                    .Build()
            };

            var traineeshipViewModel = new ApprenticeshipApplicationViewModelBuilder()
                .WithTrainingCourses(trainingCourses)
                .Build();

            var viewModel = traineeshipApplicationProvider
                .PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasTrainingCourses.Should().BeTrue();
            viewModel.Candidate.TrainingCourses.Should().Equal(trainingCourses);
        }
    }
}