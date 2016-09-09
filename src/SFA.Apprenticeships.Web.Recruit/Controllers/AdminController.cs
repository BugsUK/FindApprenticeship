namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Domain.Entities.Raa;

    [AuthorizeUser(Roles = Roles.Faa)]
    public class AdminController : RecruitmentControllerBase
    {
        public AdminController(IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangeUkprn()
        {
            return View();
        }
    }
}