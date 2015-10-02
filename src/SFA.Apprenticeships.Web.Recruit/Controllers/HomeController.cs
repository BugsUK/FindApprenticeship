namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Controllers;
    using Providers;

    [AuthorizationData]
    public class HomeController : ControllerBase<RecruitmentUserContext>
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