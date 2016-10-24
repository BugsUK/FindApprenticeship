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
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Recruit.Mediators.Application;
    using System;

    [TestFixture]
    public class ApprenticeshipApplicationMediatorTests
    {
        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnReview()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.Review(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.Review.NoApplicationId, false, false);
            logService.Verify(l => l.Info("Review vacancy failed: VacancyGuid is empty."));
        }

        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnConfirmSuccessfulDecision()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.ConfirmSuccessfulDecision(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.NoApplicationId, false, false);
            logService.Verify(l => l.Error("Confirm successful decision failed: VacancyGuid is empty."));
        }

        [Test]
        public void ShouldReturnNoVacancyIdIfNoVacancyGuidIsSuppliedOnConfirmUnsuccessfulDecision()
        {
            var logService = new Mock<ILogService>();
            var mediator = new ApprenticeshipApplicationMediator(null, null, null, null, logService.Object);

            var viewModel = new ApplicationSelectionViewModel(new VacancyApplicationsSearchViewModel(), Guid.Empty);

            var respone = mediator.ConfirmUnsuccessfulDecision(viewModel);

            respone.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.NoApplicationId, false, false);
            logService.Verify(l => l.Error("Confirm unsuccessful decision failed: VacancyGuid is empty."));
        }

        [Test]
        public void ShouldUpdateCommentsWhenSettingStatusToInProgress()
        {
            // Arrange
            var mockApplicationProvider = new Mock<IApplicationProvider>();
            var mockValidator = new Mock<ApprenticeshipApplicationViewModelServerValidator>();
            var mediator = new ApprenticeshipApplicationMediator(mockApplicationProvider.Object, mockValidator.Object, null, null, null);

            var viewModel = new Fixture().Build<ApprenticeshipApplicationViewModel>()
                .With(vm => vm.Status, ApplicationStatuses.InProgress).Create();

            mockValidator.Setup(m => m.Validate(viewModel)).Returns(new ValidationResult());

            // Act
            var response = mediator.PromoteToInProgress(viewModel);

            //Assert
            response.ViewModel.Status.Should().Be(ApplicationStatuses.InProgress);
            response.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Ok, false, false);
            mockApplicationProvider.Verify(m => m.SetStateInProgress(viewModel.ApplicationSelection), Times.Once);
            mockApplicationProvider.Verify(m => m.UpdateApprenticeshipApplicationViewModelNotes(viewModel.ApplicationSelection.ApplicationId, viewModel.Notes, false), Times.Once);
        }

        [Test]
        public void ShouldUpdateCommentsWhenSettingStatusToSubmitted()
        {
            // Arrange
            var mockApplicationProvider = new Mock<IApplicationProvider>();
            var mockValidator = new Mock<ApprenticeshipApplicationViewModelServerValidator>();
            var mediator = new ApprenticeshipApplicationMediator(mockApplicationProvider.Object, mockValidator.Object, null, null, null);

            var viewModel = new Fixture().Build<ApprenticeshipApplicationViewModel>()
                .With(vm => vm.Status, ApplicationStatuses.Submitted).Create();

            mockValidator.Setup(m => m.Validate(viewModel)).Returns(new ValidationResult());

            // Act
            var response = mediator.ReviewSetToSubmitted(viewModel);

            //Assert
            response.ViewModel.Status.Should().Be(ApplicationStatuses.Submitted);
            response.AssertCodeAndMessage(ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok, false, false);
            mockApplicationProvider.Verify(m => m.SetStateSubmitted(viewModel.ApplicationSelection), Times.Once);
            mockApplicationProvider.Verify(m => m.UpdateApprenticeshipApplicationViewModelNotes(viewModel.ApplicationSelection.ApplicationId, viewModel.Notes, false), Times.Once);
        }
    }
}