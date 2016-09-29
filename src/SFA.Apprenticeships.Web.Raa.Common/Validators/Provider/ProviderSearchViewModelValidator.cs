namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSearchViewModelClientValidator : AbstractValidator<ProviderSearchViewModel>
    {
        public ProviderSearchViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderSearchViewModelServerValidator : AbstractValidator<ProviderSearchViewModel>
    {
        public ProviderSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderSearchViewModel> validator)
        {

        }

        internal static void AddClientRules(this AbstractValidator<ProviderSearchViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderSearchViewModel> validator)
        {
            validator.RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Ukprn) || !string.IsNullOrEmpty(x.Name) || !x.PerformSearch)
                .WithMessage(ProviderSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}