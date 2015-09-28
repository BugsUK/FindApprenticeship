namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Constants;
    using Common.Mediators;
    using Constants.Messages;
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
            var viewModel = new AgencyUserViewModel();

            if (string.IsNullOrEmpty(principal?.Identity?.Name))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.EmptyUsername, viewModel, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
            }

            var username = principal.Identity.Name;

            viewModel = _agencyUserProvider.GetOrCreateAgencyUser(username);

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}