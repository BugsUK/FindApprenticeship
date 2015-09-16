namespace SFA.Apprenticeships.Web.Recruit.ViewModels.ProviderUser
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.ProviderUser;

    [Validator(typeof(ProviderUserViewModelValidator))]
    public class ProviderUserViewModel
    {
        [Display(Name = UserProfileViewModelMessages.FullnameMessages.LabelText)]
        public string Fullname { get; set; }

        [Display(Name = UserProfileViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = UserProfileViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }

        public int DefaultTrainingSiteId { get; set; }

        public bool EmailAddressVerified { get; set; }
    }
}