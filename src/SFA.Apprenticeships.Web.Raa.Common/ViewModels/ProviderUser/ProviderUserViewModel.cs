using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using System;
    using Validators.ProviderUser;
    using Web.Common.ViewModels;

    [Validator(typeof(ProviderUserViewModelValidator))]
    public class ProviderUserViewModel
    {
        public int ProviderUserId { get; set; }

        public Guid ProviderUserGuid { get; set; }

        public int ProviderId { get; set; }

        public string Ukprn { get; set; }

        public string ProviderName { get; set; }

        public string Username { get; set; }

        [Display(Name = ProviderUserViewModelMessages.FullnameMessages.LabelText)]
        public string Fullname { get; set; }

        [Display(Name = ProviderUserViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = ProviderUserViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public int DefaultProviderSiteId { get; set; }

        public bool EmailAddressVerified { get; set; }

        public ReleaseNoteViewModel ReleaseNoteViewModel { get; set; }
    }
}