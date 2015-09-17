namespace SFA.Apprenticeships.Web.Recruit.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSiteViewModelValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.NameMessages.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(ProviderSiteViewModelMessages.NameMessages.TooLongErrorText);

            RuleFor(m => m.EmailAddress)
                .Length(0, 100)
                .WithMessage(ProviderSiteViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ProviderSiteViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(ProviderSiteViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(ProviderSiteViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.PhoneNumberMessages.WhiteListErrorText);
        }
    }
}