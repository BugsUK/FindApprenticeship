namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    using System.Security.Claims;
    using Common.Mediators;
    using ViewModels;

    public interface IHomeMediator
    {
        MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal);
    }
}