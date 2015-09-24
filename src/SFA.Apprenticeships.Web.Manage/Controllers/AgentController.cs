namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Providers;

    public class AgentController : ControllerBase<RecuitmentUserContext>
    {
        public ActionResult Dashboard()
        {
            return View();
        } 
    }
}