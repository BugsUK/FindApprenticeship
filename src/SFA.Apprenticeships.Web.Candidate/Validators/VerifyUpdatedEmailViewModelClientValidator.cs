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
                .WithMessage(VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.LengthErrorText)
                .Matches(VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.WhiteListRegularExpression)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.WhiteListErrorText);

            validator.RuleFor(x => x.VerifyPassword)
                .Length(8, 127)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.RequiredErrorText)
                .Matches(VertifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListRegularExpression)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListErrorText);
        }
    }
}
