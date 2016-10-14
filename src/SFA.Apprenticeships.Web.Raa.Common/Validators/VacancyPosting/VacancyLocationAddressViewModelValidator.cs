namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyPosting
{
    using FluentValidation;
    using Constants.ViewModels;
    using ViewModels.VacancyPosting;
    using Web.Common.Validators;
    using Common = Common;

    public class VacancyLocationAddressViewModelValidator : AbstractValidator<VacancyLocationAddressViewModel>
    {
        public VacancyLocationAddressViewModelValidator()
        {
            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(VacancyLocationAddressViewModelMessages.NumberOfPositions.RequiredErrorText)
                .GreaterThanOrEqualTo(1)
                .WithMessage(VacancyLocationAddressViewModelMessages.NumberOfPositions.AtLeastOnePositionErrorText);
        }
    }

    public class VacancyLocationAddressViewModelUrlValidator : AbstractValidator<VacancyLocationAddressViewModel>
    {
        public VacancyLocationAddressViewModelUrlValidator()
        {
            this.AddCommonRules();
            this.AddUrlRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddUrlRules);
        }
    }

    internal static class VacancyLocationAddressViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<VacancyLocationAddressViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 256)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.TooLongErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText);
        }

        internal static void AddUrlRules(this AbstractValidator<VacancyLocationAddressViewModel> validator)
        {
            validator.RuleFor(x => x.OfflineApplicationUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ErrorUriText);
        }
    }
}