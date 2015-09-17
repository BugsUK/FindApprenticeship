namespace SFA.Apprenticeships.Web.Recruit.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUkprn(this IPrincipal principal)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            var ukprnClaim = claimsPrincipal?.Claims.SingleOrDefault(c => c.Type == Constants.ClaimTypes.Ukprn);
            return ukprnClaim?.Value;
        }
    }
}