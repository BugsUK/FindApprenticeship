using System.Linq;
using System.Security.Claims;
using SFA.Apprenticeships.Web.Common.Constants;
using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Recruit.Constants.Messages;
using SFA.Apprenticeships.Web.Recruit.Providers;
using SFA.Apprenticeships.Web.Recruit.ViewModels;

namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
{
    public class HomeMediator : MediatorBase, IHomeMediator
    {
        private const int MinProviderSites = 1;

        private readonly IProviderProvider _providerProvider;
        private readonly IProviderUserProvider _userProfileProvider;

        public HomeMediator(IProviderProvider providerProvider, IProviderUserProvider userProfileProvider)
        {
            _providerProvider = providerProvider;
            _userProfileProvider = userProfileProvider;
        }

        //TODO: Move to ProviderUserMediator
        public MediatorResponse<AuthorizeResponseViewModel> Authorize(ClaimsPrincipal principal)
        {
            var viewModel = new AuthorizeResponseViewModel();

            if (principal == null || principal.Identity == null || string.IsNullOrEmpty(principal.Identity.Name))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.EmptyUsername, viewModel, AuthorizeMessages.EmptyUsername, UserMessageLevel.Error);
            }

            var username = principal.Identity.Name;
            viewModel.Username = username;
            var userProfile = _userProfileProvider.GetUserProfileViewModel(username);
            if (userProfile != null)
            {
                viewModel.EmailAddress = userProfile.EmailAddress;
                viewModel.EmailAddressVerified = userProfile.EmailAddressVerified;
            }

            if (!principal.HasClaim(c => c.Type == Constants.ClaimTypes.Ukprn))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingProviderIdentifier, viewModel, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
            }

            if (!principal.IsInRole(Constants.Roles.Faa))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingServicePermission, viewModel, AuthorizeMessages.MissingServicePermission, UserMessageLevel.Warning);
            }

            var ukprn = principal.Claims.Single(c => c.Type == Constants.ClaimTypes.Ukprn).Value;
            if (string.IsNullOrEmpty(ukprn))
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.MissingProviderIdentifier, viewModel, AuthorizeMessages.MissingProviderIdentifier, UserMessageLevel.Error);
            }

            var provider = _providerProvider.GetProviderViewModel(ukprn);

            if (provider == null)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.NoProviderProfile, viewModel, AuthorizeMessages.NoProviderProfile, UserMessageLevel.Warning);
            }

            if (provider.ProviderSiteViewModels.Count() < MinProviderSites)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.FailedMinimumSitesCountCheck, viewModel, AuthorizeMessages.FailedMinimumSitesCountCheck, UserMessageLevel.Warning);
            }

            if (userProfile == null)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.NoUserProfile, viewModel, AuthorizeMessages.NoUserProfile, UserMessageLevel.Info);
            }

            if (!userProfile.EmailAddressVerified)
            {
                return GetMediatorResponse(HomeMediatorCodes.Authorize.EmailAddressNotVerified, viewModel, AuthorizeMessages.EmailAddressNotVerified, UserMessageLevel.Info);
            }

            return GetMediatorResponse(HomeMediatorCodes.Authorize.Ok, viewModel);
        }
    }
}