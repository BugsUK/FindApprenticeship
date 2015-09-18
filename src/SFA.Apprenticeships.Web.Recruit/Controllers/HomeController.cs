using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Common.Providers;
using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Common.Framework;
    using Constants;
    using Providers;
    using ViewModels;

    public class HomeController : ControllerBase<RecuitmentUserContext>
    {
        // private readonly IAuthenticationTicketService _authenticationTicketService;
        private readonly IHomeMediator _homeMediator;
        private readonly ICookieAuthorizationDataProvider _authorizationDataProvider;

        public HomeController(IHomeMediator homeMediator, ICookieAuthorizationDataProvider authorizationDataProvider)
        {
            // _authenticationTicketService = authenticationTicketService;
            _homeMediator = homeMediator;
            _authorizationDataProvider = authorizationDataProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        //TODO: Probably move to ProviderUserController
        public ActionResult Authorize()
        {
            //TODO: ACS Calls this action during signout. Need to suppress it in a cleaner manner
            if (!Request.IsAuthenticated)
            {
                return null;
            }

            var claimsPrincipal = (ClaimsPrincipal)User;
            var response = _homeMediator.Authorize(claimsPrincipal);
            var message = response.Message;
            var viewModel = response.ViewModel;

            //Add domain claims
            if (viewModel.EmailAddress != null)
            {
                AddClaim(System.Security.Claims.ClaimTypes.Email, viewModel.EmailAddress, viewModel);
            }
            if (viewModel.EmailAddressVerified)
            {
                AddClaim(System.Security.Claims.ClaimTypes.Role, Roles.VerifiedEmail, viewModel);
            }

            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }

            switch (response.Code)
            {
                case HomeMediatorCodes.Authorize.EmptyUsername:
                case HomeMediatorCodes.Authorize.MissingProviderIdentifier:
                case HomeMediatorCodes.Authorize.MissingServicePermission:
                    _authorizationDataProvider.Clear(HttpContext);
                    return RedirectToRoute(RecruitmentRouteNames.SignOut, new { returnRoute = RecruitmentRouteNames.LandingPage });
                case HomeMediatorCodes.Authorize.NoProviderProfile:
                case HomeMediatorCodes.Authorize.FailedMinimumSitesCountCheck:
                case HomeMediatorCodes.Authorize.FirstUser:
                    return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);
                case HomeMediatorCodes.Authorize.NoUserProfile:
                    return RedirectToRoute(RecruitmentRouteNames.Settings);
                case HomeMediatorCodes.Authorize.EmailAddressNotVerified:
                    return RedirectToRoute(RecruitmentRouteNames.VerifyEmail);
                case HomeMediatorCodes.Authorize.Ok:
                    var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);
                    if (returnUrl.IsValidReturnUrl())
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        private void AddClaim(string type, string value, AuthorizeResponseViewModel viewModel)
        {
            var claim = new Claim(type, value);
            _authorizationDataProvider.AddClaim(claim, HttpContext, viewModel.Username);
        }
    }
}