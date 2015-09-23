using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Common.Providers;
using SFA.Apprenticeships.Web.Manage.Mediators.Home;

namespace SFA.Apprenticeships.Web.Manage.Controllers
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

            //Clear existing claims
            _authorizationDataProvider.Clear(HttpContext);

            //Add domain claims
            if (viewModel.EmailAddress != null)
            {
                AddClaim(System.Security.Claims.ClaimTypes.Email, viewModel.EmailAddress, viewModel);
            }

            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }

            switch (response.Code)
            {
                case HomeMediatorCodes.Authorize.Ok:
                    var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);
                    if (returnUrl.IsValidReturnUrl())
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }
                    return RedirectToRoute(ManagementRouteNames.ManagementHome);

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