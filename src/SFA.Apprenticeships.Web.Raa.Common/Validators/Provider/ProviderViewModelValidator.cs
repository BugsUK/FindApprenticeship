using FluentValidation;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    public class ProviderViewModelClientValidator : AbstractValidator<ProviderViewModel>
    {
        public ProviderViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderViewModelServerValidator : AbstractValidator<ProviderViewModel>
    {
        public ProviderViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderViewModel> validator)
        {
            validator.RuleFor(m => m.Ukprn)
                .NotEmpty()
                .WithMessage(ProviderViewModelMessages.Ukprn.RequiredErrorText)
                .Length(8, 8)
                .WithMessage(ProviderViewModelMessages.Ukprn.RequiredLengthErrorText)
                .Matches(ProviderViewModelMessages.Ukprn.WhiteListRegularExpression)
                .WithMessage(ProviderViewModelMessages.Ukprn.WhiteListErrorText);

            validator.RuleFor(m => m.FullName)
                .NotEmpty()
                .WithMessage(ProviderViewModelMessages.FullName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderViewModelMessages.FullName.TooLongErrorText)
                .Matches(ProviderViewModelMessages.FullName.WhiteListRegularExpression)
                .WithMessage(ProviderViewModelMessages.FullName.WhiteListErrorText);

            validator.RuleFor(m => m.TradingName)
                .NotEmpty()
                .WithMessage(ProviderViewModelMessages.TradingName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderViewModelMessages.TradingName.TooLongErrorText)
                .Matches(ProviderViewModelMessages.TradingName.WhiteListRegularExpression)
                .WithMessage(ProviderViewModelMessages.TradingName.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<ProviderViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderViewModel> validator)
        {
            
        }
    }
}