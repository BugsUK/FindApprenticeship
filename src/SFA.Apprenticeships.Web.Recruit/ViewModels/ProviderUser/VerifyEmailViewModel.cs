namespace SFA.Apprenticeships.Web.Recruit.ViewModels.ProviderUser
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.ProviderUser;

    [Validator(typeof(VerifyEmailViewModelValidator))]
    public class VerifyEmailViewModel
    {
        [Display(Name = VerifyEmailViewModelMessages.VerificationCode.LabelText)]
        public string VerificationCode { get; set; }

        public string EmailAddress { get; set; }
    }
}