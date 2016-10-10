namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class VacancyOwnerRelationshipViewModelValidator : AbstractValidator<VacancyOwnerRelationshipViewModel>
    {
        public VacancyOwnerRelationshipViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.EmployerDescription)
                .NotEmpty()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerDescription.RequiredErrorText)
                .Matches(VacancyOwnerRelationshipViewModelMessages.EmployerDescription.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerDescription.WhiteListInvalidTagErrorText);

            RuleFor(x => x.EmployerWebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerWebsiteUrl.ErrorUriText)
                .When(x => !string.IsNullOrEmpty(x.EmployerWebsiteUrl));

            RuleFor(x => x.IsEmployerLocationMainApprenticeshipLocation)
                .NotNull()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.IsEmployerLocationMainApprenticeshipLocation.RequiredErrorText);

            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.NumberOfPositions.RequiredErrorText)
                .GreaterThanOrEqualTo(1)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.NumberOfPositions.LengthErrorText)
                .When(x => x.IsEmployerLocationMainApprenticeshipLocation.HasValue && x.IsEmployerLocationMainApprenticeshipLocation == true);
        }
    }
}