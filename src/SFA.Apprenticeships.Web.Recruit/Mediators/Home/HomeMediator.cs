using System.Linq;
using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Constants;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Recruit.Constants.Messages;
using SFA.Apprenticeships.Web.Recruit.Providers;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public class HomeMediator : MediatorBase, IHomeMediator
    {
        private const int MinProviderSites = 1;

        private readonly IProviderProvider _providerProvider;

        public HomeMediator(IProviderProvider providerProvider)
        {
            _providerProvider = providerProvider;
        }

        public MediatorResponse Authorize(ClaimsPrincipal principal)
        {
            if (principal == null || principal.Identity == null || string.IsNullOrEmpty(principal.Identity.Name))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.EmptyUsername, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
            }

            if (!principal.HasClaim(c => c.Type == Constants.ClaimTypes.Ukprn))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
            }

            if (!principal.IsInRole(Constants.Roles.Faa))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingServicePermission, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
            }

            var ukprn = principal.Claims.Single(c => c.Type == Constants.ClaimTypes.Ukprn).Value;
            if (string.IsNullOrEmpty(ukprn))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingProviderIdentifier, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
            }

            var provider = _providerProvider.GetProviderViewModel(ukprn);
            if (provider == null)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.NoProviderProfile, AuthorizeMessages.NoProviderProfile, UserMessageLevel.Warning);
            }

            if (provider.ProviderSiteViewModels.Count() < MinProviderSites)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.FailedMinimumSitesCountCheck, AuthorizeMessages.FailedMinimumSitesCountCheck, UserMessageLevel.Warning);
            }

            //var providerSites = _providerSiteProvider.GetProviderSites();

            return GetMediatorResponse(HomeMediatorCodes.Authorize.Ok);
        }
    }
}