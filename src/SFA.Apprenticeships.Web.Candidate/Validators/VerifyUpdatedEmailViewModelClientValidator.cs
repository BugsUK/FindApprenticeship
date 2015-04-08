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
                .Length(4, 4)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.VerifyUpdatedEmailCodeMessages.LengthErrorText);

            validator.RuleFor(x => x.Password)
                .Length(8, 127)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.RequiredErrorText)
                .Matches(VertifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListRegularExpression)
                .WithMessage(VertifyUpdatedEmailViewModelMessages.PasswordMessages.WhiteListErrorText);
        }
    }
}
