namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Attributes;
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
        public FileContentResult VacanciesListCsv(ReportVacanciesParameters parameters)
        {
            var csvBytes = _reportingMediator.GetVacanciesListReportBytes(parameters);
            return File(csvBytes, "text/csv", "VacanciesList.csv");
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult SuccessfulCandidatesCsv()
        {
            return View(_reportingMediator.GetSuccessfulCandidatesReportParams());
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public FileContentResult SuccessfulCandidatesCsv(ReportSuccessfulCandidatesParameters parameters)
        {
            var csvBytes = _reportingMediator.GetSuccessfulCandidatesReportBytes(parameters);
            return File(csvBytes, "text/csv", "SuccessfulCandidates.csv");
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.Raa)]
        public ActionResult UnsuccessfulCandidatesCsv()
        {
            return View(_reportingMediator.GetUnsuccessfulCandidatesReportParams());
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public FileContentResult UnsuccessfulCandidatesCsv(ReportUnsuccessfulCandidatesParameters parameters)
        {
            var csvBytes = _reportingMediator.GetUnsuccessfulCandidatesReportBytes(parameters);
            return File(csvBytes, "text/csv", "UnsuccessfulCandidatesCsv.csv");
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

        [HttpPost]
        [AuthorizeUser(Roles = Roles.Raa)]
        public FileContentResult VacancyExtensionsCsv(ReportVacancyExtensionsParameters parameters)
        {
            var csvBytes = _reportingMediator.GetVacancyExtensionsReportBytes(parameters);
            return File(csvBytes, "text/csv", "VacancyExtensions.csv");
        }
    }
}