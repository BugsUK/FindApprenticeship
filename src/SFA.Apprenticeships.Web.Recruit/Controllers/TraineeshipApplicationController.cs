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
    using Raa.Common.ViewModels.Application.Traineeship;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

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
        public ActionResult ReviewSaveAndContinue(TraineeshipApplicationViewModel traineeshipApplicationViewModel)
        {
            var response = _traineeshipApplicationMediator.ReviewSaveAndContinue(traineeshipApplicationViewModel);
            var viewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error:
                    return View("Review", response.ViewModel);

                case TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return RedirectToRoute(RecruitmentRouteNames.ReviewTraineeshipApplication, viewModel);

                case TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyApplications, viewModel.ApplicationSelection.RouteValues);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}
