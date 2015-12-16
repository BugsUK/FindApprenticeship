namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using Mediators.Application;
    using ViewModels.Application.Apprenticeship;

    public class ApprenticeshipApplicationController : RecruitmentControllerBase
    {
        private readonly IApprenticeshipApplicationMediator _apprenticeshipApplicationMediator;

        public ApprenticeshipApplicationController(IApprenticeshipApplicationMediator apprenticeshipApplicationMediator)
        {
            _apprenticeshipApplicationMediator = apprenticeshipApplicationMediator;
        }

        [HttpGet]
        public ActionResult Review(Guid applicationId)
        {
            var response = _apprenticeshipApplicationMediator.Review(applicationId);

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
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, new { applicationId = viewModel.ApplicationId });

                case ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, new { applicationId = viewModel.ApplicationId });

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
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, new { applicationId = viewModel.ApplicationId });

                case ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, new { applicationId = viewModel.ApplicationId });

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
                    return RedirectToRoute(RecruitmentRouteNames.ReviewApprenticeshipApplication, new { applicationId = viewModel.ApplicationId });

                case ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, new { vacancyReferenceNumber = viewModel.Vacancy.VacancyReferenceNumber });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}