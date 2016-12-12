namespace SFA.Apprenticeships.Infrastructure.Common.CurrentUser
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using Application.Interfaces;

    public class CurrentUserService : ICurrentUserService
    {
        public string CurrentUserName => Thread.CurrentPrincipal.Identity.Name;

        public bool IsInRole(string role) => Thread.CurrentPrincipal.IsInRole(role);

        public string GetClaimValue(string type)
        {
            return ((ClaimsPrincipal)Thread.CurrentPrincipal).Claims.LastOrDefault(c => c.Type == type)?.Value;
        }
    }
}
