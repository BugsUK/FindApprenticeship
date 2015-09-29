namespace SFA.Apprenticeships.Web.Common.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using ClaimTypes = Constants.ClaimTypes;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUkprn(this IPrincipal principal)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            var ukprnClaim = claimsPrincipal?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Ukprn);
            return ukprnClaim?.Value;
        }

        public static string GetRoleList(this IPrincipal principal)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            var roleListClaim = claimsPrincipal?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.RoleList);
            return roleListClaim?.Value;
        }
    }
}