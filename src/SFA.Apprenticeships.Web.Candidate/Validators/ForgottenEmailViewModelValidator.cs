namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Register;

    public class ForgottenEmailViewModelClientValidator : AbstractValidator<ForgottenEmailViewModel>
    {
        public ForgottenEmailViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ForgottenEmailViewModelServerValidator : AbstractValidator<ForgottenEmailViewModel>
    {
        public ForgottenEmailViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public static class ForgottenEmailViewModelValidationRules
    {
        public static void AddCommonRules(this AbstractValidator<ForgottenEmailViewModel> validator)
        {
            validator.RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.WhiteListErrorText);
        }
    }
}