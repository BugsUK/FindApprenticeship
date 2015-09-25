namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Mediators;
    using Providers;
    using ViewModels;

    public class AgencyUserMediator : MediatorBase, IAgencyUserMediator
    {
        private readonly IAgencyUserProvider _agencyUserProvider;

        public AgencyUserMediator(IAgencyUserProvider agencyUserProvider)
        {
            _agencyUserProvider = agencyUserProvider;
        }

        public MediatorResponse<AgencyUserViewModel> Authorize(ClaimsPrincipal principal)
        {
            var username = principal.Identity.Name;

            var viewModel = _agencyUserProvider.GetOrCreateAgencyUser(username);

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}