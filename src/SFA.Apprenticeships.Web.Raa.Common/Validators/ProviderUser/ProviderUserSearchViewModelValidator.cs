namespace SFA.Apprenticeships.Web.Raa.Common.Validators.ProviderUser
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.ProviderUser;

    public class ProviderUserSearchViewModelClientValidator : AbstractValidator<ProviderUserSearchViewModel>
    {
        public ProviderUserSearchViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderUserSearchViewModelServerValidator : AbstractValidator<ProviderUserSearchViewModel>
    {
        public ProviderUserSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderUserSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderUserSearchViewModel> validator)
        {

        }

        internal static void AddClientRules(this AbstractValidator<ProviderUserSearchViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderUserSearchViewModel> validator)
        {
            validator.RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Username) || !string.IsNullOrEmpty(x.Name) || !string.IsNullOrEmpty(x.Email) || x.AllUnverifiedEmails || !x.PerformSearch)
                .WithMessage(ProviderUserSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}