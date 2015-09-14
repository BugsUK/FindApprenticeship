using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public class HomeMediator : IHomeMediator
    {
        public MediatorResponse Authorize(ClaimsPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.HasClaim(c => c.Type == Constants.ClaimTypes.Ukprn))
            {
                return new MediatorResponse {Code = HomeMediatorCodes.Authorize.MissingProviderIdentifier};
            }

            if (!principal.IsInRole(Constants.Roles.Faa))
            {
                return new MediatorResponse { Code = HomeMediatorCodes.Authorize.MissingServicePermission };
            }

            return new MediatorResponse {Code = HomeMediatorCodes.Authorize.Ok};
        }
    }
}