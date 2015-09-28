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
    }
}
