using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Constants;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Recruit.Constants.Messages;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public class HomeMediator : MediatorBase, IHomeMediator
    {
        public MediatorResponse Authorize(ClaimsPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.HasClaim(c => c.Type == Constants.ClaimTypes.Ukprn))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
            }

            if (!principal.IsInRole(Constants.Roles.Faa))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(HomeMediatorCodes.Authorize.Ok);
        }
    }
}