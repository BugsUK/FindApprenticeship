namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Domain.Entities.Raa;
    using Mediators.Application;
    using Raa.Common.ViewModels.Application;
    using Raa.Common.ViewModels.Application.Apprenticeship;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

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

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewAppointCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
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

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "Review")]
        public ActionResult ReviewRejectCandidate(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
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
        public ActionResult ReviewSaveAndExit(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.ReviewSaveAndExit(apprenticeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message);
            }

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Error:
                    return View("Review", response.ViewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, viewModel);

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok:
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
                    if (response.Message != null)
                    {
                        SetUserMessage(response.Message);
                    }

                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, response.ViewModel.RouteValues);

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

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SendUnsuccessfulDecision")]
        public ActionResult SendUnsuccessfulDecision(ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel)
        {
            var response = _apprenticeshipApplicationMediator.SendUnsuccessfulDecision(apprenticeshipApplicationViewModel.ApplicationSelection);

            switch (response.Code)
            {
                case ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok:
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
