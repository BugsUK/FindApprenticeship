namespace SFA.Apprenticeships.Web.Manage.ViewModels.ProviderUser
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.ProviderUser;

    [Validator(typeof(ProviderUserViewModelValidator))]
    public class ProviderUserViewModel
    {
        [Display(Name = ProviderUserViewModelMessages.FullnameMessages.LabelText)]
        public string Fullname { get; set; }

        [Display(Name = ProviderUserViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = ProviderUserViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public string DefaultTrainingSiteErn { get; set; }

        public bool EmailAddressVerified { get; set; }
    }
}