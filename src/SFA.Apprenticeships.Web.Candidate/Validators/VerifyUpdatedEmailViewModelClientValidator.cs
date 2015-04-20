namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Account;

    public class VerifyUpdatedEmailViewModelServerValidator : AbstractValidator<VerifyUpdatedEmailViewModel>
    {
        public VerifyUpdatedEmailViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public class VerifyUpdatedEmailViewModelClientValidator : AbstractValidator<VerifyUpdatedEmailViewModel>
    {
        public VerifyUpdatedEmailViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class VerifyUpdatedEmailViewModelValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<VerifyUpdatedEmailViewModel> validator)
        {
            validator.RuleFor(model => model.PendingUsernameCode)
                .NotEmpty()
                .WithMessage(VerifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(VerifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.LengthErrorText)
                .Matches(VerifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.WhiteListRegularExpression)
                .WithMessage(VerifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.WhiteListErrorText);

            validator.RuleFor(x => x.VerifyPassword)
                .Length(8, 127)
                .WithMessage(VerifyUpdatedEmailViewModelMessages.PasswordMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(VerifyUpdatedEmailViewModelMessages.PasswordMessages.RequiredErrorText)
                .Matches(VerifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListRegularExpression)
                .WithMessage(VerifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListErrorText);
        }
    }
}
