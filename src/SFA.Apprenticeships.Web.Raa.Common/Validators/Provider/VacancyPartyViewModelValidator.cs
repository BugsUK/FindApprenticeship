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
            RuleFor(x => x.EmployerDescription)
                .NotEmpty()
                .WithMessage(VacancyPartyViewModelMessages.EmployerDescription.RequiredErrorText)
                .Matches(VacancyPartyViewModelMessages.EmployerDescription.WhiteListRegularExpression)
                .WithMessage(VacancyPartyViewModelMessages.EmployerDescription.WhiteListErrorText);

            RuleFor(x => x.EmployerWebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyPartyViewModelMessages.EmployerWebsiteUrl.ErrorUriText)
                .When(x => !string.IsNullOrEmpty(x.EmployerWebsiteUrl));

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