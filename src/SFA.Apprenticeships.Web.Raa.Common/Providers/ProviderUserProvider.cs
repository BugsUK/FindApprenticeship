using SFA.Apprenticeships.Application.Interfaces.Users;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.ReferenceData;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Web.Common.ViewModels;

    public class ProviderUserProvider : IProviderUserProvider
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderService _providerService;
        private readonly IProviderUserAccountService _providerUserAccountService;
        private readonly IReferenceDataService _referenceDataService;

        public ProviderUserProvider(
            IUserProfileService userProfileService,
            IProviderService providerService,
            IProviderUserAccountService providerUserAccountService,
            IReferenceDataService referenceDataService)
        {
            _userProfileService = userProfileService;
            _providerService = providerService;
            _providerUserAccountService = providerUserAccountService;
            _referenceDataService = referenceDataService;
        }

        public ProviderUserViewModel GetUserProviderViewModel(int providerUserId)
        {
            var providerUser = _userProfileService.GetProviderUser(providerUserId);
            return Convert(providerUser);
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

        public ProviderUser GetProviderUser(string username)
        {
            return _userProfileService.GetProviderUser(username);
        }

        public void DismissReleaseNotes(string username, int version)
        {
            var providerUser = _userProfileService.GetProviderUser(username);
            providerUser.ReleaseNoteVersion = version;
            _userProfileService.UpdateProviderUser(providerUser);
        }

        public IEnumerable<ProviderUserViewModel> GetProviderUsers(string ukprn)
        {
            var providerUsers = _userProfileService.GetProviderUsers(ukprn);
            return providerUsers.Select(Convert);
        }

        public ProviderUserSearchResultsViewModel SearchProviderUsers(ProviderUserSearchViewModel searchViewModel)
        {
            var searchParameters = new ProviderUserSearchParameters
            {
                Username = searchViewModel.Username,
                Name = searchViewModel.Name,
                Email = searchViewModel.Email,
                AllUnverifiedEmails = searchViewModel.AllUnverifiedEmails
            };

            var providerUsers = _userProfileService.SearchProviderUsers(searchParameters);
            
            var viewModel = new ProviderUserSearchResultsViewModel
            {
                SearchViewModel = searchViewModel,
                ProviderUsers = providerUsers.Select(Convert).ToList()
            };

            return viewModel;
        }

        public bool ValidateEmailVerificationCode(string username, string code)
        {
            var providerUser = _userProfileService.GetProviderUser(username);

            if (!code.Equals(providerUser.EmailVerificationCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            providerUser.Status = ProviderUserStatus.EmailVerified;
            providerUser.EmailVerificationCode = null;
            providerUser.EmailVerifiedDate = DateTime.UtcNow;

            _userProfileService.UpdateProviderUser(providerUser);

            return true;
        }

        public ProviderUserViewModel SaveProviderUser(
            string username, string ukprn, ProviderUserViewModel providerUserViewModel)
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
                    Status = ProviderUserStatus.Registered
                };
            }

            var shouldSendEmailVerificationCode = isNewProviderUser || !string.Equals(providerUser.Email, providerUserViewModel.EmailAddress, StringComparison.CurrentCultureIgnoreCase);

            providerUser.Username = username;
            providerUser.Status = providerUser.Status;
            providerUser.Fullname = providerUserViewModel.Fullname;
            providerUser.Email = providerUserViewModel.EmailAddress;
            providerUser.PhoneNumber = providerUserViewModel.PhoneNumber;
            providerUser.PreferredProviderSiteId = providerUserViewModel.DefaultProviderSiteId;

            var savedProviderUser = isNewProviderUser
                ? _userProfileService.CreateProviderUser(providerUser)
                : _userProfileService.UpdateProviderUser(providerUser);

            if (shouldSendEmailVerificationCode)
            {
                _providerUserAccountService.SendEmailVerificationCode(username);
            }

            return Convert(savedProviderUser);
        }

        public void ResendEmailVerificationCode(string username)
        {
            _providerUserAccountService.ResendEmailVerificationCode(username);
        }

        private ProviderUserViewModel Convert(ProviderUser providerUser)
        {
            ReleaseNoteViewModel releaseNoteViewModel = null;
            var releaseNotes = _referenceDataService.GetReleaseNotes(DasApplication.Recruit);
            var releaseNote = releaseNotes?.LastOrDefault(rn => rn.Version > providerUser.ReleaseNoteVersion);
            if (releaseNote != null)
            {
                releaseNoteViewModel = new ReleaseNoteViewModel
                {
                    Version = releaseNote.Version,
                    Note = releaseNote.Note
                };
            }

            var viewModel = new ProviderUserViewModel
            {
                ProviderUserId = providerUser.ProviderUserId,
                ProviderUserGuid = providerUser.ProviderUserGuid,
                ProviderId = providerUser.ProviderId,
                Ukprn = providerUser.Ukprn,
                ProviderName = providerUser.ProviderName,
                Username = providerUser.Username,
                DefaultProviderSiteId = providerUser.PreferredProviderSiteId ?? 0,
                EmailAddress = providerUser.Email,
                EmailAddressVerified = providerUser.Status == ProviderUserStatus.EmailVerified,
                Fullname = providerUser.Fullname,
                PhoneNumber = providerUser.PhoneNumber,
                CreatedDateTime = providerUser.CreatedDateTime,
                ReleaseNoteViewModel = releaseNoteViewModel
            };

            return viewModel;
        }
    }
}