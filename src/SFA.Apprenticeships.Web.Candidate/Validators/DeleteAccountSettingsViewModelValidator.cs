namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Account;

    public class DeleteAccountSettingsViewModelClientValidator : AbstractValidator<DeleteAccountSettingsViewModel>
    {
        public DeleteAccountSettingsViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class DeleteAccountSettingsViewModelServerValidator : AbstractValidator<DeleteAccountSettingsViewModel>
    {
        public DeleteAccountSettingsViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public static class DeleteAccountSettingsViewModelValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<DeleteAccountSettingsViewModel> validator)
        {
            validator.RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage(SettingsViewModelMessages.EmailAddressMessages.RequiredErrorText);

            validator.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(SettingsViewModelMessages.PasswordMessages.RequiredErrorText);
        }
    }
}