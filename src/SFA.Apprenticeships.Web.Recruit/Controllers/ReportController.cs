namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Domain.Entities.Raa;
    using Mediators.Report;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Report;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.VerifiedEmail)]
    public class ReportController : RecruitmentControllerBase
    {
        private readonly IReportMediator _reportMediator;

        public ReportController(IReportMediator reportMediator, IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
            _reportMediator = reportMediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ReportMenu());
        }

        [HttpGet]
        public ActionResult ApplicationsReceived()
        {
            return View(new ApplicationsReceivedParameters());
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationsReceived")]
        [HttpPost]
        public ActionResult ValidateApplicationsReceived(ApplicationsReceivedParameters parameters)
        {
            var response = _reportMediator.ValidateApplicationsReceivedParameters(parameters);
            switch (response.Code)
            {
                case ReportMediatorCodes.ValidateApplicationsReceivedParameters.Ok:
                    return View("ApplicationsReceived", response.ViewModel);
                case ReportMediatorCodes.ValidateApplicationsReceivedParameters.ValidationError:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("ApplicationsReceived", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "ApplicationsReceived")]
        [HttpPost]
        public ActionResult DownloadApplicationsReceived(ApplicationsReceivedParameters parameters)
        {
            var response = _reportMediator.GetApplicationsReceived(parameters, User.Identity.Name);
            return File(response.ViewModel, "text/csv", "ApplicationsReceived.csv");
        }

        [HttpGet]
        public ActionResult CandidatesWithApplications()
        {
            return View(new CandidatesWithApplicationsParameters());
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CandidatesWithApplications")]
        [HttpPost]
        public ActionResult ValidateCandidatesWithApplications(CandidatesWithApplicationsParameters parameters)
        {
            var response = _reportMediator.ValidateCandidatesWithApplicationsParameters(parameters);
            switch (response.Code)
            {
                case ReportMediatorCodes.ValidateCandidatesWithApplicationsParameters.Ok:
                    return View("CandidatesWithApplications", response.ViewModel);
                case ReportMediatorCodes.ValidateCandidatesWithApplicationsParameters.ValidationError:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("CandidatesWithApplications", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CandidatesWithApplications")]
        [HttpPost]
        public ActionResult DownloadCandidatesWithApplications(CandidatesWithApplicationsParameters parameters)
        {
            var response = _reportMediator.GetCandidatesWithApplications(parameters, User.Identity.Name);
            return File(response.ViewModel, "text/csv", "CandidatesWithApplications.csv");
        }
    }
}