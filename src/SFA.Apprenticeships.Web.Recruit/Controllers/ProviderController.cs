namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Providers;

    public class ProviderController : ControllerBase<RecuitmentUserContext>
    {
        public ActionResult Sites()
        {
            UserData.Push(UserMessageConstants.InfoMessage, "As you're the first person to sign in from your organisation, please take a moment to review your training sites.");
            return View();
        }
    }
}