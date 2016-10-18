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
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using System;
    using System.Web.Mvc;

    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    [AuthorizeUser(Roles = Roles.Faa)]
    public class ApprenticeshipApplicationController : RecruitmentControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator, IConfigurationService configurationService, ILogService logService)
            : base(configurationService, logService)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
        }

        [HttpGet]
        public ActionResult Review(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _apprenticeshipApplicationMediator.Review(applicationSelectionViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.Review.Ok:
                    return View(response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.Review.NoApplicationId:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewAppointCandidate(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmSuccessfulApprenticeshipApplication, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private ActionResult ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewRejectCandidate(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmUnsuccessfulApprenticeshipApplication, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewRevertToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewRevertToInProgress(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmRevertToInProgress, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewSaveAndContinue(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            switch (apprenticeshipApplicationViewModel.Status)
            {
                case ApplicationStatuses.Submitted:
                    return SetToSubmitted(apprenticeshipApplicationViewModel);
                case ApplicationStatuses.InProgress:
                    return PromoteToInProgress(apprenticeshipApplicationViewModel);
                case ApplicationStatuses.Successful:
                    return ReviewAppointCandidate(apprenticeshipApplicationViewModel);
                case ApplicationStatuses.Unsuccessful:
                    return ReviewRejectCandidate(apprenticeshipApplicationViewModel);
                default:
                    throw new InvalidOperationException("Invalid status change");
            }
        }

        private ActionResult PromoteToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.PromoteToInProgress(apprenticeshipApplicationViewModel);
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

        private ActionResult SetToSubmitted(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewSetToSubmitted(apprenticeshipApplicationViewModel);
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

        [HttpGet]
        public ActionResult ConfirmSuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ConfirmSuccessfulDecision(applicationSelectionViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.Ok:
                    return View(response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.NoApplicationId:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SendSuccessfulDecision")]
        public ActionResult SendSuccessfulDecision(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.SendSuccessfulDecision(apprenticeshipApplicationViewModel.ApplicationSelection);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.SendSuccessfulDecision.Ok:
                    return View("SentDecisionConfirmation", response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmUnsuccessfulDecision(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ConfirmUnsuccessfulDecision(applicationSelectionViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok:
                    return View(response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.NoApplicationId:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SendUnsuccessfulDecision")]
        public ActionResult SendUnsuccessfulDecision(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.SendUnsuccessfulDecision(apprenticeshipApplicationViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok:
                    return View("SentDecisionConfirmation", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmRevertToInProgress(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ConfirmRevertToInProgress(applicationSelectionViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.Ok:
                    return View(response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.NoApplicationId:
                    SetUserMessage(response.Message);
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "RevertToInProgress")]
        public ActionResult RevertToInProgress(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.RevertToInProgress(apprenticeshipApplicationViewModel.ApplicationSelection);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.RevertToInProgress.Ok:
                    if (response.Message != null)
                    {
                        SetUserMessage(response.Message.Text);
                    }

                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, response.ViewModel.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}
