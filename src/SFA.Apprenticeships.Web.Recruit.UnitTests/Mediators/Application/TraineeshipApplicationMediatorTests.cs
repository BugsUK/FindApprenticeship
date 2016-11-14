namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Common.UnitTests.Mediators;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;
    using Recruit.Mediators.Application;
    using System;

    [TestFixture]
    public class TraineeshipApplicationMediatorTests
    {
        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnReview()
        {
            var logService = new Mock<ILogService>();
            var mediator = new TraineeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.Review(viewModel);

            respone.AssertCodeAndMessage(TraineeshipApplicationMediatorCodes.Review.NoApplicationId, false, false);
            logService.Verify(l => l.Info("Review vacancy failed: VacancyGuid is empty."));
        }


        [Test]
        public void ShouldUpdateCommentsWhenSettingStatusToInProgress()
        {
            // Arrange
            var mockApplicationProvider = new Mock<IApplicationProvider>();
            var mockValidator = new Mock<TraineeshipApplicationViewModelServerValidator>();
            var mediator = new TraineeshipApplicationMediator(mockApplicationProvider.Object, mockValidator.Object, null, null, null);
            var viewModel = new Fixture().Build<TraineeshipApplicationViewModel>()
                .With(vm => vm.Status, ApplicationStatuses.InProgress).Create();
            mockValidator.Setup(m => m.Validate(viewModel)).Returns(new ValidationResult());
            // Act
            var response = mediator.PromoteToInProgress(viewModel);
            //Assert
            response.ViewModel.Status.Should().Be(ApplicationStatuses.InProgress);
            response.AssertCodeAndMessage(TraineeshipApplicationMediatorCodes.PromoteToInProgress.Ok, false, false);
            mockApplicationProvider.Verify(m => m.SetTraineeshipStateInProgress(viewModel.ApplicationSelection), Times.Once);
            mockApplicationProvider.Verify(m => m.UpdateTraineeshipApplicationViewModelNotes(viewModel.ApplicationSelection.ApplicationId, viewModel.Notes, false), Times.Once);
        }

        [Test]
        public void ShouldUpdateCommentsWhenSettingStatusToSubmitted()
        {
            // Arrange
            var mockApplicationProvider = new Mock<IApplicationProvider>();
            var mockValidator = new Mock<TraineeshipApplicationViewModelServerValidator>();
            var mediator = new TraineeshipApplicationMediator(mockApplicationProvider.Object, mockValidator.Object, null, null, null);
            var viewModel = new Fixture().Build<TraineeshipApplicationViewModel>()
                .With(vm => vm.Status, ApplicationStatuses.Submitted).Create();
            mockValidator.Setup(m => m.Validate(viewModel)).Returns(new ValidationResult());
            // Act
            var response = mediator.ReviewSetToSubmitted(viewModel);
            //Assert
            response.ViewModel.Status.Should().Be(ApplicationStatuses.Submitted);
            response.AssertCodeAndMessage(TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok, false, false);
            mockApplicationProvider.Verify(m => m.SetTraineeshipStateSubmitted(viewModel.ApplicationSelection), Times.Once);
            mockApplicationProvider.Verify(m => m.UpdateTraineeshipApplicationViewModelNotes(viewModel.ApplicationSelection.ApplicationId, viewModel.Notes, false), Times.Once);
        }
    }
}