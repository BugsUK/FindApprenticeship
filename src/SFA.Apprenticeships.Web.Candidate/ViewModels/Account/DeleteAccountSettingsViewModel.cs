namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Account
{
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using System.ComponentModel.DataAnnotations;
    using Validators;

    [Validator(typeof(DeleteAccountSettingsViewModelClientValidator))]
    public class DeleteAccountSettingsViewModel
    {
        [Display(Name = SettingsViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = SettingsViewModelMessages.PasswordMessages.LabelText)]
        public string Password { get; set; }

    }
}