namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Account;

    public class EmailViewModelServerValidator : AbstractValidator<EmailViewModel>
    {
        public EmailViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public class EmailViewModelClientValidator : AbstractValidator<EmailViewModel>
    {
        public EmailViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class EmailValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<EmailViewModel> validator)
        {
            validator.RuleFor(model => model.EmailAddress)
                .Length(0, 100)
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.WhiteListErrorText);
        }
    }
}
