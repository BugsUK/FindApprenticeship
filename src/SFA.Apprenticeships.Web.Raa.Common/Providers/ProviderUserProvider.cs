using System;
using System.Collections.Generic;
using System.Linq;
using SFA.Apprenticeships.Application.Interfaces.Users;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Domain.Entities.Raa.Users;

    public class ProviderUserProvider : IProviderUserProvider
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderUserAccountService _providerUserAccountService;

        public ProviderUserProvider(
            IUserProfileService userProfileService,
            IProviderUserAccountService providerUserAccountService)
        {
            _userProfileService = userProfileService;
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
                providerUser.Status = ProviderUserStatus.EmailVerified;
                _userProfileService.UpdateUser(providerUser);

                return true;
            }

            return false;

            //End Stub
        }

        public ProviderUserViewModel SaveProviderUser(string username, string ukprn, ProviderUserViewModel providerUserViewModel)
        {
            var isNewProvider = false;
            var providerUser = _userProfileService.GetProviderUser(username);

            if (providerUser == null)
            {
                providerUser = new ProviderUser();
                isNewProvider = true;
            }

            var emailChanged = !string.Equals(providerUser.Email, providerUserViewModel.EmailAddress, StringComparison.CurrentCultureIgnoreCase);

            //TODO: Probably put this in a strategy in the service and add the verify email code
            providerUser.Username = username;
            providerUser.Email = providerUserViewModel.EmailAddress;
            providerUser.Fullname = providerUserViewModel.Fullname;
            providerUser.PhoneNumber = providerUserViewModel.PhoneNumber;
            int? defaultProviderSiteId = null;
            if (providerUserViewModel.DefaultProviderSiteId > 0)
            {
                defaultProviderSiteId = providerUserViewModel.DefaultProviderSiteId;
            }
            providerUser.PreferredProviderSiteId = defaultProviderSiteId;
            providerUser.Status = providerUser.Status;

            if (isNewProvider)
            {
                _userProfileService.CreateUser(providerUser);
            }
            else
            {
                _userProfileService.UpdateUser(providerUser);
            }

            if (emailChanged)
            {
                // TODO: AG: US824: call to this service must be moved. Note that this service is responsible for generating the email
                // verification code and updating the user again so take care must be taken around other calls to UserProfileService.CreateUser().
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
                DefaultProviderSiteId = providerUser.PreferredProviderSiteId ?? 0,
                EmailAddress = providerUser.Email,
                EmailAddressVerified = providerUser.Status == ProviderUserStatus.EmailVerified,
                Fullname = providerUser.Fullname,
                PhoneNumber = providerUser.PhoneNumber
            };

            return viewModel;
        }
    }
}