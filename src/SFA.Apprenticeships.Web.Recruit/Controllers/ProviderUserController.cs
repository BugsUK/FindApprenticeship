namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Providers;

    public class ProviderUserController : ControllerBase<RecuitmentUserContext>
    { 
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult UserInfo()
        {
            return View();
        }
    }
}