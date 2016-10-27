namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa;
    using Mediators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Traineeship;
    using System;
    using System.Web.Mvc;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class TraineeshipApplicationController : RecruitmentControllerBase
    {
        private readonly ITraineeshipApplicationMediator _traineeshipApplicationMediator;

        public TraineeshipApplicationController(
            ITraineeshipApplicationMediator traineeshipApplicationMediator,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _traineeshipApplicationMediator = traineeshipApplicationMediator;
        }

        [HttpGet]
        public ActionResult Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _traineeshipApplicationMediator.Review(applicationSelectionViewModel);

            switch (response.Code)
            {
                case TraineeshipApplicationMediatorCodes.Review.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewSaveAndExit(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            switch (traineeshipApplicationViewModel.Status)
            {
                case ApplicationStatuses.Submitted:
                    return SetToSubmitted(traineeshipApplicationViewModel);
                case ApplicationStatuses.InProgress:
                    return PromoteToInProgress(traineeshipApplicationViewModel);
                default:
                    throw new InvalidOperationException("Invalid status change");
            }
        }

        private ActionResult PromoteToInProgress(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var response = _traineeshipApplicationMediator.PromoteToInProgress(traineeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult SetToSubmitted(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var response = _traineeshipApplicationMediator.ReviewSetToSubmitted(traineeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}
