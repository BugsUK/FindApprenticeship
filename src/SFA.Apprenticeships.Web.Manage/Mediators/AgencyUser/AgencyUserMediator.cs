namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Mediators;
    using ViewModels;

    public class AgencyUserMediator : MediatorBase, IAgencyUserMediator
    {
        public MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal)
        {
            var viewModel = new AuthorizeResponseViewModel();

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}