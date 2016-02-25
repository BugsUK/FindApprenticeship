namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Mediators.Application;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public class ApprenticeshipApplicationController : RecruitmentControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator)
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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewAppointCandidate(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ConfirmSuccessfulApprenticeshipApplication, viewModel.ApplicationSelection);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewRejectCandidate(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel.ApplicationSelection);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewSaveAndExit(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewSaveAndExit(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, viewModel.ApplicationSelection);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmSuccessful(ApplicationSelectionViewModel applicationSelectionViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ConfirmSuccessful(applicationSelectionViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ConfirmSuccessful.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "AppointCandidate")]
        public ActionResult ConfirmSuccessfulAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ConfirmSuccessfulAppointCandidate(apprenticeshipApplicationViewModel);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.AppointCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}
