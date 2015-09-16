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
                .WithMessage(UserProfileViewModelMessages.FullnameMessages.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(UserProfileViewModelMessages.FullnameMessages.TooLongErrorText);

            RuleFor(m => m.EmailAddress)
                .Length(0, 100)
                .WithMessage(UserProfileViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(UserProfileViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(UserProfileViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(UserProfileViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(UserProfileViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(UserProfileViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(UserProfileViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(UserProfileViewModelMessages.PhoneNumberMessages.WhiteListErrorText);
        }
    }
}