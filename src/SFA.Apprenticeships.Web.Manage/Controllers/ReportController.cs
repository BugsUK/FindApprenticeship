namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Validators.Extensions;
    using Domain.Entities.Raa;
    using Mediators.Reporting;
    using ViewModels;

    [AuthorizeUser(Roles = Roles.Raa)]
    public class ReportController : ManagementControllerBase
    {
        private readonly IReportingMediator _reportingMediator;

        public ReportController(IReportingMediator reportingMediator)
        {
            _reportingMediator = reportingMediator;
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult VacanciesListCsv()
        {
            return View(new ReportVacanciesParameters());
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult VacanciesListCsv(ReportVacanciesParameters parameters)
        {
            var validationResponse = _reportingMediator.Validate(parameters);
            switch (validationResponse.Code)
            {
                case ReportingMediatorCodes.ReportCodes.Ok:
                    var response = _reportingMediator.GetVacanciesListReportBytes(parameters);
                    switch (response.Code)
                    {
                        case ReportingMediatorCodes.ReportCodes.Ok:
                            return File(response.ViewModel, "text/csv", "VacanciesList.csv");
                        case ReportingMediatorCodes.ReportCodes.Error:
                        default:
                            ModelState.Clear();
                            return View(parameters);
                    }
                case ReportingMediatorCodes.ReportCodes.ValidationError:
                default:
                    ModelState.Clear();
                    validationResponse.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(validationResponse.ViewModel);
            }
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult SuccessfulCandidatesCsv()
        {
            var response = _reportingMediator.GetSuccessfulCandidatesReportParams();
            if (response.Code != ReportingMediatorCodes.ReportCodes.Ok)
                RedirectToAction("Index");

            return View(response.ViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult SuccessfulCandidatesCsv(ReportSuccessfulCandidatesParameters parameters)
        {
            var validationResponse = _reportingMediator.Validate(parameters);
            var newParameterSet = _reportingMediator.GetUnsuccessfulCandidatesReportParams();
            validationResponse.ViewModel.ManagedByList = newParameterSet.ViewModel.ManagedByList;
            validationResponse.ViewModel.RegionList = newParameterSet.ViewModel.RegionList;
            switch (validationResponse.Code)
            {
                case ReportingMediatorCodes.ReportCodes.Ok:
                    var response = _reportingMediator.GetSuccessfulCandidatesReportBytes(parameters);
                    switch (response.Code)
                    {
                        case ReportingMediatorCodes.ReportCodes.Ok:
                            return File(response.ViewModel, "text/csv", "SuccessfulCandidates.csv");
                        case ReportingMediatorCodes.ReportCodes.Error:
                        default:
                            ModelState.Clear();
                            return View(parameters);
                    }
                case ReportingMediatorCodes.ReportCodes.ValidationError:
                default:
                    ModelState.Clear();
                    validationResponse.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(validationResponse.ViewModel);
            }
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult UnsuccessfulCandidatesCsv()
        {
            var response = _reportingMediator.GetUnsuccessfulCandidatesReportParams();
            if (response.Code != ReportingMediatorCodes.ReportCodes.Ok)
                RedirectToAction("Index");

            return View(response.ViewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "UnsuccessfulCandidatesCsv")]
        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult ValidateUnsuccessfulCandidatesCsv(ReportUnsuccessfulCandidatesParameters parameters)
        {
            var validationResponse = _reportingMediator.Validate(parameters);
            var newParameterSet = _reportingMediator.GetUnsuccessfulCandidatesReportParams();
            validationResponse.ViewModel.ManagedByList = newParameterSet.ViewModel.ManagedByList;
            validationResponse.ViewModel.RegionList = newParameterSet.ViewModel.RegionList;
            switch (validationResponse.Code)
            {
                case ReportingMediatorCodes.ReportCodes.Ok:
                    return View(validationResponse.ViewModel);
                case ReportingMediatorCodes.ReportCodes.ValidationError:
                default:
                    ModelState.Clear();
                    validationResponse.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("UnsuccessfulCandidatesCsv", validationResponse.ViewModel);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "UnsuccessfulCandidatesCsv")]
        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult DownloadUnsuccessfulCandidatesCsv(ReportUnsuccessfulCandidatesParameters parameters)
        {
            var response = _reportingMediator.GetUnsuccessfulCandidatesReportBytes(parameters);
            return File(response.ViewModel, "text/csv", "UnsuccessfulCandidatesCsv.csv");
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult Index()
        {
            return View(new ReportMenu());
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult VacancyExtensionsCsv()
        {
            return View(new ReportVacancyExtensionsParameters());
        }

/*        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult VacancyExtensionsCsv(ReportVacancyExtensionsParameters parameters)
        {
            var validationResponse = _reportingMediator.Validate(parameters);
            switch (validationResponse.Code)
            {
                case ReportingMediatorCodes.ReportCodes.Ok:
                    var response = _reportingMediator.GetVacancyExtensionsReportBytes(parameters);
                    switch (response.Code)
                    {
                        case ReportingMediatorCodes.ReportCodes.Ok:
                            return File(response.ViewModel, "text/csv", "VacancyExtensions.csv");
                        case ReportingMediatorCodes.ReportCodes.Error:
                        default:
                            ModelState.Clear();
                            return View(parameters);
                    }
                case ReportingMediatorCodes.ReportCodes.ValidationError:
                default:
                    ModelState.Clear();
                    validationResponse.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(validationResponse.ViewModel);
            }
        }*/

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyExtensionsCsv")]
        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult ValidateVacancyExtensionsCsv(ReportVacancyExtensionsParameters parameters)
        {
            var validationResponse = _reportingMediator.Validate(parameters);
            
            switch (validationResponse.Code)
            {
                case ReportingMediatorCodes.ReportCodes.Ok:
                    return View(validationResponse.ViewModel);
                case ReportingMediatorCodes.ReportCodes.ValidationError:
                default:
                    ModelState.Clear();
                    validationResponse.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View("VacancyExtensionsCsv", validationResponse.ViewModel);
            }
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyExtensionsCsv")]
        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult DownloadVacancyExtensionsCsv(ReportVacancyExtensionsParameters parameters)
        {
            var response = _reportingMediator.GetVacancyExtensionsReportBytes(parameters);
            return File(response.ViewModel, "text/csv", "VacancyExtensionsCsv.csv");
        }
    }
}