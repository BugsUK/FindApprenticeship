namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Common.Constants.Pages;
    using Common.ViewModels.Candidate;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class PatchApplicationViewModelTests
    {
        [Test]
        public void NullSavedViewModel()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();

            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();

            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, null, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().Be(ApplicationPageMessages.SubmitApplicationFailed);
            viewModel.HasError().Should().BeTrue();
        }

        [Test]
        public void QualificationChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();

            var savedTraineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();
            var qualifications = new List<QualificationsViewModel> { new QualificationsViewModelBuilder().WithSubject("Maths").WithGrade("C").WithYear("2015").Build() };
            var monitoringInformationViewModel = new Fixture().Build<MonitoringInformationViewModel>().Create();
            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().WithQualifications(qualifications).WithMonitoringInformation(monitoringInformationViewModel).Build();

            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasQualifications.Should().BeTrue();
            viewModel.Candidate.Qualifications.Should().Equal(qualifications);
        }

        [Test]
        public void WorkExperienceChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();
            var savedTraineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();

            var workExperience = new List<WorkExperienceViewModel>
            {
                new WorkExperienceViewModelBuilder().WithDescription("Work").WithEmployer("Employer").Build() 
            };

            var monitoringInformationViewModel = new Fixture().Build<MonitoringInformationViewModel>().Create();
            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder().WithWorkExperience(workExperience).WithMonitoringInformation(monitoringInformationViewModel).Build();
            var viewModel = traineeshipApplicationProvider.PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasWorkExperience.Should().BeTrue();
            viewModel.Candidate.WorkExperience.Should().Equal(workExperience);
        }

        [Test]
        public void TrainingCourseChanges()
        {
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();
            var savedTraineeshipViewModel = new TraineeshipApplicationViewModelBuilder().Build();

            var trainingCourses = new List<TrainingCourseViewModel>
            {
                new TrainingCourseViewModelBuilder()
                    .WithProvider("Provider")
                    .Build()
            };

            var monitoringInformationViewModel = new Fixture().Build<MonitoringInformationViewModel>().Create();

            var traineeshipViewModel = new TraineeshipApplicationViewModelBuilder()
                .WithTrainingCourses(trainingCourses)
                .WithMonitoringInformation(monitoringInformationViewModel)
                .Build();

            var viewModel = traineeshipApplicationProvider
                .PatchApplicationViewModel(candidateId, savedTraineeshipViewModel, traineeshipViewModel);

            viewModel.Should().NotBeNull();
            viewModel.ViewModelMessage.Should().BeNull();
            viewModel.HasError().Should().BeFalse();
            viewModel.Candidate.HasTrainingCourses.Should().BeTrue();
            viewModel.Candidate.TrainingCourses.Should().Equal(trainingCourses);
        }

        [TestCase(false, false, null, null)]
        [TestCase(false, true, null, null)]
        [TestCase(false, true, "A", "A")]
        [TestCase(true, false, "B", "")]
        [TestCase(true, true, "C", "C")]
        public void ShouldPatchRequiresSupportForInterview(
            bool isJavascript,
            bool requiresSupportForInterview,
            string anythingWeCanDoToSupportYourInterview,
            string expectedAythingWeCanDoToSupportYourInterview)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();
            var savedModel = new TraineeshipApplicationViewModelBuilder().Build();

            var submittedModel = new TraineeshipApplicationViewModelBuilder()
                .IsJavascript(isJavascript)
                .WithMonitoringInformation(new MonitoringInformationViewModel
                {
                    RequiresSupportForInterview = requiresSupportForInterview,
                    AnythingWeCanDoToSupportYourInterview = anythingWeCanDoToSupportYourInterview
                })
                .Build();

            // Act.
            var patchedViewModel = traineeshipApplicationProvider
                .PatchApplicationViewModel(candidateId, savedModel, submittedModel);

            patchedViewModel.Should().NotBeNull();
            patchedViewModel.ViewModelMessage.Should().BeNull();
            patchedViewModel.HasError().Should().BeFalse();

            // Assert.
            patchedViewModel.Candidate.MonitoringInformation.AnythingWeCanDoToSupportYourInterview
                .Should().Be(expectedAythingWeCanDoToSupportYourInterview);
        }

        [Test]
        public void ShouldPatchMonitoringInformation()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var traineeshipApplicationProvider = new TraineeshipApplicationProviderBuilder().Build();
            var savedViewModel = new TraineeshipApplicationViewModelBuilder().Build();

            var monitoringInformationViewModel = new Fixture()
                .Build<MonitoringInformationViewModel>()
                .Create();

            var submittedViewModel = new TraineeshipApplicationViewModelBuilder()
                .WithMonitoringInformation(monitoringInformationViewModel)
                .Build();

            // Act.
            var patchedViewModel = traineeshipApplicationProvider
                .PatchApplicationViewModel(candidateId, savedViewModel, submittedViewModel);

            patchedViewModel.Should().NotBeNull();
            patchedViewModel.ViewModelMessage.Should().BeNull();
            patchedViewModel.HasError().Should().BeFalse();

            // Assert.
            patchedViewModel.Candidate.MonitoringInformation.Should().Be(monitoringInformationViewModel);
        }
    }
}