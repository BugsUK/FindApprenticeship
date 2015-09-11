namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Providers;

    public class HomeController : ControllerBase<RecuitmentUserContext>
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}