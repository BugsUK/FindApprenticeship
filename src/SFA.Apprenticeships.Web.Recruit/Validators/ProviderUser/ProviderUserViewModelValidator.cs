namespace SFA.Apprenticeships.Web.Recruit.Validators.ProviderUser
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;
    using ViewModels.ProviderUser;

    public class ProviderUserViewModelValidator : AbstractValidator<ProviderUserViewModel>
    {
        public ProviderUserViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.Fullname)
                .NotEmpty()
                .WithMessage(ProviderUserViewModelMessages.FullnameMessages.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(ProviderUserViewModelMessages.FullnameMessages.TooLongErrorText)
                .Matches(ProviderUserViewModelMessages.FullnameMessages.WhiteListRegularExpression)
                .WithMessage(ProviderUserViewModelMessages.FullnameMessages.WhiteListErrorText);

            RuleFor(m => m.EmailAddress)
                .Length(0, 100)
                .WithMessage(ProviderUserViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ProviderUserViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ProviderUserViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ProviderUserViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(ProviderUserViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(ProviderUserViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(ProviderUserViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(ProviderUserViewModelMessages.PhoneNumberMessages.WhiteListErrorText);
        }
    }
}