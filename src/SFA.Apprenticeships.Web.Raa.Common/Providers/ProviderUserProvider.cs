namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Users;
    using Domain.Entities.Users;
    using ViewModels.ProviderUser;
    using Application.Interfaces.Providers;

    public class ProviderUserProvider : IProviderUserProvider
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderService _providerService;
        private readonly IProviderUserAccountService _providerUserAccountService;

        public ProviderUserProvider(
            IUserProfileService userProfileService,
            IProviderService providerService,
            IProviderUserAccountService providerUserAccountService)
        {
            _userProfileService = userProfileService;
            _providerService = providerService;
            _providerUserAccountService = providerUserAccountService;
        }

        public ProviderUserViewModel GetUserProfileViewModel(string username)
        {
            var providerUser = _userProfileService.GetProviderUser(username);
            if (providerUser != null)
            {
                return Convert(providerUser);
            }

            return null;
        }

        public IEnumerable<ProviderUserViewModel> GetUserProfileViewModels(string ukprn)
        {
            var providerUsers = _userProfileService.GetProviderUsers(ukprn);
            return providerUsers.Select(Convert);
        }

        public bool ValidateEmailVerificationCode(string username, string code)
        {
            var providerUser = _userProfileService.GetProviderUser(username);

            if (code.Equals(providerUser.EmailVerificationCode, StringComparison.CurrentCultureIgnoreCase))
            {
                //TODO: Probably put all this in a strategy in the service
                providerUser.EmailVerificationCode = null;
                providerUser.EmailVerifiedDate = DateTime.UtcNow;
                providerUser.Status = ProviderUserStatuses.EmailVerified;

                _userProfileService.UpdateProviderUser(providerUser);

                return true;
            }

            return false;
        }

        public ProviderUserViewModel SaveProviderUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel)
        {
            var providerUser = _userProfileService.GetProviderUser(username);
            var isNewProviderUser = providerUser == null;

            if (isNewProviderUser)
            {
                var provider = _providerService.GetProvider(ukprn);

                providerUser = new ProviderUser
                {
                    ProviderId = provider.ProviderId,
                    ProviderUserGuid = Guid.NewGuid(),
                    Status = ProviderUserStatuses.Registered
                };
            }

            var shouldSendEmailVerificationCode = isNewProviderUser || !string.Equals(providerUser.Email, providerUserViewModel.EmailAddress, StringComparison.CurrentCultureIgnoreCase);

            providerUser.Username = username;
            providerUser.Email = providerUserViewModel.EmailAddress;
            providerUser.Fullname = providerUserViewModel.Fullname;
            providerUser.PhoneNumber = providerUserViewModel.PhoneNumber;
            providerUser.PreferredSiteErn = providerUserViewModel.DefaultProviderSiteErn;
            providerUser.Status = providerUser.Status;

            if (isNewProviderUser)
            {
                _userProfileService.CreateProviderUser(providerUser);
            }
            else
            {
                _userProfileService.UpdateProviderUser(providerUser);
            }

            if (shouldSendEmailVerificationCode)
            {
                _providerUserAccountService.SendEmailVerificationCode(username);
            }

            return GetUserProfileViewModel(providerUser.Username);
        }

        public void ResendEmailVerificationCode(string username)
        {
            _providerUserAccountService.ResendEmailVerificationCode(username);
        }

        private static ProviderUserViewModel Convert(ProviderUser providerUser)
        {
            var viewModel = new ProviderUserViewModel
            {
                DefaultProviderSiteErn = providerUser.PreferredSiteErn,
                EmailAddress = providerUser.Email,
                EmailAddressVerified = providerUser.Status == ProviderUserStatuses.EmailVerified,
                Fullname = providerUser.Fullname,
                PhoneNumber = providerUser.PhoneNumber
            };

            return viewModel;
        }
    }
}
