namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Constants;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Models.Azure.AccessControlService;
    using Common.Providers.Azure.AccessControlService;
    using Constants.Messages;
    using Providers;
    using ViewModels;

    public class AgencyUserMediator : MediatorBase, IAgencyUserMediator
    {
        private readonly IAgencyUserProvider _agencyUserProvider;
        private readonly IAuthorizationErrorProvider _authorizationErrorProvider;

        public AgencyUserMediator(
            IAgencyUserProvider agencyUserProvider,
            IAuthorizationErrorProvider authorizationErrorProvider)
        {
            _agencyUserProvider = agencyUserProvider;
            _authorizationErrorProvider = authorizationErrorProvider;
        }

        public MediatorResponse<AgencyUserViewModel> Authorize(ClaimsPrincipal principal)
        {
            var viewModel = new AgencyUserViewModel();

            if (string.IsNullOrEmpty(principal?.Identity?.Name))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.EmptyUsername, viewModel, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
            }

            if (!principal.IsInRole(Constants.Roles.Raa))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.MissingServicePermission, viewModel, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Error);
            }

            var roleList = principal.GetRoleList();
            if (string.IsNullOrEmpty(roleList))
            {
                return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.MissingRoleListClaim, viewModel, AuthorizeMessages.MissingRoleListClaim, UserMessageLevel.Error);
            }

            var username = principal.Identity.Name;
            viewModel = _agencyUserProvider.GetOrCreateAgencyUser(username, roleList);

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }

        public AuthorizationErrorDetailsViewModel AuthorizationError(string errorDetails)
        {
            return _authorizationErrorProvider.GetAuthorizationErrorDetailsViewModel(errorDetails);
        }

        public MediatorResponse<AgencyUserViewModel> GetAgencyUser(ClaimsPrincipal principal)
        {
            var username = principal.Identity.Name;
            var roleList = principal.GetRoleList();
            var viewModel = _agencyUserProvider.GetAgencyUser(username, roleList);

            return GetMediatorResponse(AgencyUserMediatorCodes.GetAgencyUser.Ok, viewModel);
        }

        public MediatorResponse<AgencyUserViewModel> SaveAgencyUser(ClaimsPrincipal principal, AgencyUserViewModel viewModel)
        {
            var username = principal.Identity.Name;
            var roleList = principal.GetRoleList();
            viewModel = _agencyUserProvider.SaveAgencyUser(username, roleList, viewModel);

            return GetMediatorResponse(AgencyUserMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}
