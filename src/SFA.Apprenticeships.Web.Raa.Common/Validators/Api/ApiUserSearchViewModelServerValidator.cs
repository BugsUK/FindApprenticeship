namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Api
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Api;

    public class ApiUserSearchViewModelClientValidator : AbstractValidator<ApiUserSearchViewModel>
    {
        public ApiUserSearchViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ApiUserSearchViewModelServerValidator : AbstractValidator<ApiUserSearchViewModel>
    {
        public ApiUserSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ApiUserSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ApiUserSearchViewModel> validator)
        {

        }

        internal static void AddClientRules(this AbstractValidator<ApiUserSearchViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ApiUserSearchViewModel> validator)
        {
            validator.RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.ExternalSystemId) || !string.IsNullOrEmpty(x.Id) || !string.IsNullOrEmpty(x.Name) || !x.PerformSearch)
                .WithMessage(ApiUserSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}