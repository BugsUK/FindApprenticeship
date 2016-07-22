namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
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
    }
}