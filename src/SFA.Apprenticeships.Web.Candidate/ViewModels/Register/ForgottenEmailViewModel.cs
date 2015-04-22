namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(ForgottenEmailViewModelClientValidator))]
    public class ForgottenEmailViewModel
    {
        [Display(Name = ForgottenEmailViewModelMessages.PhoneNumberMessages.LabelText,
            Description = ForgottenEmailViewModelMessages.PhoneNumberMessages.HintText)]
        public string PhoneNumber { get; set; }
    }
}