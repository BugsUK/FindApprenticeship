namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;

    public class HomeController : ManagementControllerBase
    {
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