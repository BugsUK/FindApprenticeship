namespace SFA.Apprenticeships.Web.Manage.Validators.ProviderUser
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;
    using ViewModels.ProviderUser;

    public class VerifyEmailViewModelValidator : AbstractValidator<VerifyEmailViewModel>
    {
        public VerifyEmailViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.VerificationCode)
                .NotEmpty()
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.LengthErrorText)
                .Matches(VerifyEmailViewModelMessages.VerificationCode.WhiteListRegularExpression)
                .WithMessage(VerifyEmailViewModelMessages.VerificationCode.WhiteListErrorText);
        }
    }
}