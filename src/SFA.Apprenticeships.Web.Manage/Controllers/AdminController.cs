namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Attributes;
    using Domain.Entities.Raa;

    [AuthorizeUser(Roles = Roles.Raa)]
    [AuthorizeUser(Roles = Roles.Admin)]
    public class AdminController : ManagementControllerBase
    {
        public AdminController(IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}