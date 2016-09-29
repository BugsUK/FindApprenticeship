namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSiteSearchViewModelClientValidator : AbstractValidator<ProviderSiteSearchViewModel>
    {
        public ProviderSiteSearchViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderSiteSearchViewModelServerValidator : AbstractValidator<ProviderSiteSearchViewModel>
    {
        public ProviderSiteSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderSiteSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderSiteSearchViewModel> validator)
        {

        }

        internal static void AddClientRules(this AbstractValidator<ProviderSiteSearchViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderSiteSearchViewModel> validator)
        {
            validator.RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.EdsUrn) || !string.IsNullOrEmpty(x.Name) || !x.PerformSearch)
                .WithMessage(ProviderSiteSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}