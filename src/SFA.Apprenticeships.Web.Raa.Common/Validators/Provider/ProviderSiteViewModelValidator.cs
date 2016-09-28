namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSiteViewModelClientValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderSiteViewModelServerValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderSiteViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {
            validator.RuleFor(m => m.EdsUrn)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.RequiredErrorText)
                .Length(9, 9)
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.RequiredLengthErrorText)
                .Matches(ProviderSiteViewModelMessages.EdsUrn.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.WhiteListErrorText);

            validator.RuleFor(m => m.FullName)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.FullName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.FullName.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.FullName.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.FullName.WhiteListErrorText);

            validator.RuleFor(m => m.TradingName)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.TradingName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.TradingName.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.TradingName.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.TradingName.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }
    }
}