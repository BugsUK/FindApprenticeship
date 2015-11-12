namespace SFA.Apprenticeships.Web.Manage.Mediators.AgencyUser
{
    using System.Security.Claims;
    using Common.Mediators;
    using Common.Models.Azure.AccessControlService;
    using ViewModels;

    public interface IAgencyUserMediator
    {
        MediatorResponse<AgencyUserViewModel> Authorize(ClaimsPrincipal principal);

        AuthorizationErrorDetailsViewModel AuthorizationError(string errorDetails);

        MediatorResponse<AgencyUserViewModel> GetAgencyUser(ClaimsPrincipal principal);

        MediatorResponse<AgencyUserViewModel> SaveAgencyUser(ClaimsPrincipal principal, AgencyUserViewModel viewModel);

        MediatorResponse<HomeViewModel> GetHomeViewModel(ClaimsPrincipal principal);
    }
}
