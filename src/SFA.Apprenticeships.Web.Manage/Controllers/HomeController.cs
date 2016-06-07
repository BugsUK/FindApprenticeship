namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using SFA.Infrastructure.Interfaces;
    using System.Web.Mvc;

    public class HomeController : ManagementControllerBase
    {
        public HomeController(ILogService logService) : base(logService)
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