namespace SFA.Apprenticeships.Web.Recruit.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using ClaimTypes = Common.Constants.ClaimTypes;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUkprn(this IPrincipal principal)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            var ukprnClaim = claimsPrincipal?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Ukprn);
            return ukprnClaim?.Value;
        }
    }
}