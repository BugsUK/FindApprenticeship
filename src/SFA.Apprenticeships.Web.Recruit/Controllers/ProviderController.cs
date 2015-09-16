using SFA.Apprenticeships.Web.Recruit.Constants;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Mediators.Provider;
    using Providers;

    [Authorize(Roles = Roles.Faa)]
    public class ProviderController : ControllerBase<RecuitmentUserContext>
    {
        private readonly IProviderMediator _providerMediator;

        public ProviderController(
            IProviderMediator providerMediator)
        {
            _providerMediator = providerMediator;
        }

        [HttpGet]
        public ActionResult Sites()
        {
            UserData.Push(UserMessageConstants.InfoMessage, "As you're the first person to sign in from your organisation, please take a moment to review your training sites.");

            return View();
        }

        [HttpGet]
        public ActionResult AddSite()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditSite()
        {
            return View();
        }
    }
}