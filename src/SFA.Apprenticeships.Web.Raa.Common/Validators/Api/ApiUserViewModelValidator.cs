namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Api
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Api;

    public class ApiUserViewModelClientValidator : AbstractValidator<ApiUserViewModel>
    {
        public ApiUserViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ApiUserViewModelServerValidator : AbstractValidator<ApiUserViewModel>
    {
        public ApiUserViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ApiUserViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ApiUserViewModel> validator)
        {
            validator.RuleFor(m => m.Password)
                .Length(16)
                .WithMessage(ApiUserViewModelMessages.Password.RequiredLengthErrorText)
                .Matches(ApiUserViewModelMessages.Password.WhiteListRegularExpression)
                .WithMessage(ApiUserViewModelMessages.Password.WhiteListErrorText);

            validator.RuleFor(m => m.CompanyId)
                .NotEmpty()
                .WithMessage(ApiUserViewModelMessages.CompanyId.RequiredErrorText)
                .Length(8, 9)
                .WithMessage(ApiUserViewModelMessages.CompanyId.RequiredLengthErrorText)
                .Matches(ApiUserViewModelMessages.CompanyId.WhiteListRegularExpression)
                .WithMessage(ApiUserViewModelMessages.CompanyId.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<ApiUserViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ApiUserViewModel> validator)
        {

        }
    }
}