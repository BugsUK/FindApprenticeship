namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class VacancyPartyViewModelValidator : AbstractValidator<VacancyPartyViewModel>
    {
        public VacancyPartyViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(VacancyPartyViewModelMessages.Description.RequiredErrorText)
                .Matches(VacancyPartyViewModelMessages.Description.WhiteListRegularExpression)
                .WithMessage(VacancyPartyViewModelMessages.Description.WhiteListErrorText);

            RuleFor(x => x.WebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyPartyViewModelMessages.WebsiteUrl.ErrorUriText)
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));

            RuleFor(x => x.IsEmployerLocationMainApprenticeshipLocation)
                .NotNull()
                .WithMessage(VacancyPartyViewModelMessages.IsEmployerLocationMainApprenticeshipLocation.RequiredErrorText);

            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(VacancyPartyViewModelMessages.NumberOfPositions.RequiredErrorText)
                .GreaterThanOrEqualTo(1)
                .WithMessage(VacancyPartyViewModelMessages.NumberOfPositions.LengthErrorText)
                .When(x => x.IsEmployerLocationMainApprenticeshipLocation.HasValue && x.IsEmployerLocationMainApprenticeshipLocation == true);
        }
    }
}