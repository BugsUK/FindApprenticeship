namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using SFA.Infrastructure.Interfaces;
    using System.Web.Mvc;

    using SFA.Apprenticeships.Application.Interfaces;

    public class HomeController : ManagementControllerBase
    {
        public HomeController(IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
    }
}