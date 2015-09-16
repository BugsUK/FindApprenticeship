namespace SFA.Apprenticeships.Web.Recruit.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class UserProfileViewModel
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