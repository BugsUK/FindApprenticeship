using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Manage.ViewModels;

namespace SFA.Apprenticeships.Web.Manage.Mediators.Home
{
    public class HomeMediator : MediatorBase, IHomeMediator
    {
        //TODO: Move to ProviderUserMediator
        public MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal)
        {
            var viewModel = new AuthorizeResponseViewModel();

            return GetMediatorResponse(HomeMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}