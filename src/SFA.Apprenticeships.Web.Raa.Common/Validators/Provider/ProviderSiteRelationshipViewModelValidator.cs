namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSiteRelationshipViewModelClientValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteRelationshipViewModelClientValidator()
        {
            this.AddRelationshipCommonRules();
            this.AddRelationshipClientRules();
        }
    }

    public class ProviderSiteRelationshipViewModelServerValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteRelationshipViewModelServerValidator()
        {
            this.AddRelationshipCommonRules();
            this.AddRelationshipServerRules();
        }
    }

    internal static class ProviderSiteRelationshipViewModelValidatorRules
    {
        internal static void AddRelationshipCommonRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {
            validator.RuleFor(m => m.ProviderUkprn)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.ProviderUkprn.RequiredErrorText)
                .Length(8, 8)
                .WithMessage(ProviderSiteViewModelMessages.ProviderUkprn.RequiredLengthErrorText)
                .Matches(ProviderSiteViewModelMessages.ProviderUkprn.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.ProviderUkprn.WhiteListErrorText);
        }

        internal static void AddRelationshipClientRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }

        internal static void AddRelationshipServerRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }
    }
}