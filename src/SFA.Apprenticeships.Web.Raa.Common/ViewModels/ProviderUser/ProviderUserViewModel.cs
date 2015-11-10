using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Recruit.Validators.ProviderUser;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    [Validator(typeof(ProviderUserViewModelValidator))]
    public class ProviderUserViewModel
    {
        [Display(Name = ProviderUserViewModelMessages.FullnameMessages.LabelText)]
        public string Fullname { get; set; }

        [Display(Name = ProviderUserViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = ProviderUserViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public string DefaultProviderSiteErn { get; set; }

        public bool EmailAddressVerified { get; set; }
    }
}