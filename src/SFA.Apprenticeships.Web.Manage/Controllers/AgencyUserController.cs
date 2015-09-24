namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Attributes;
    using Common.Constants;
    using Common.Framework;
    using Common.Mediators;
    using Constants;
    using Mediators.AgencyUser;
    using ControllerBase = Common.Controllers.ControllerBase;

    [AuthorizeUser(Roles = Constants.Roles.Raa)]
    public class AgencyUserController : ControllerBase
    {
        private readonly IAgencyUserMediator _agencyUserMediator;

        public AgencyUserController(IAgencyUserMediator agencyUserMediator)
        {
            _agencyUserMediator = agencyUserMediator;
        }

        public ActionResult Authorize()
        {
            //TODO: ACS Calls this action during signout. Need to suppress it in a cleaner manner
            if (!Request.IsAuthenticated)
            {
                return null;
            }

            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _agencyUserMediator.Authorize(claimsPrincipal);

            switch (response.Code)
            {
                case AgencyUserMediatorCodes.Authorize.Ok:
                    var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);
                    if (returnUrl.IsValidReturnUrl())
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }
                    return RedirectToRoute(ManagementRouteNames.Dashboard);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}