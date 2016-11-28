namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
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
                .WithMessage(
                    VacancyOwnerRelationshipViewModelMessages.EmployerDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerDescription.WhiteListInvalidTagErrorText)
                .When(x => x.IsAnonymousEmployer.HasValue && x.IsAnonymousEmployer == false);

            RuleFor(x => x.EmployerWebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.EmployerWebsiteUrl.ErrorUriText)
                .When(x => !string.IsNullOrEmpty(x.EmployerWebsiteUrl));

            RuleFor(x => x.EmployerApprenticeshipLocation)
                .NotNull()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.IsEmployerLocationMainApprenticeshipLocation.RequiredErrorText);

            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.NumberOfPositions.RequiredErrorText)
                .GreaterThanOrEqualTo(1)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.NumberOfPositions.LengthErrorText)
                .When(x => x.EmployerApprenticeshipLocation == VacancyLocationType.SpecificLocation);

            RuleFor(x => x.IsAnonymousEmployer)
                .NotNull()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.IsAnonymousEmployer.RequiredErrorText);

            RuleFor(x => x.AnonymousEmployerDescription)
                 .NotEmpty()
                 .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerDescription.RequiredErrorText)
                 .Matches(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerDescription.WhiteListHtmlRegularExpression)
                 .WithMessage(
                     VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerDescription.WhiteListInvalidCharacterErrorText)
                 .Must(Common.BeAValidFreeText)
                 .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerDescription.WhiteListInvalidTagErrorText)
                 .When(IsAnonymousEmployer);

            RuleFor(x => x.AnonymousEmployerReason)
                .NotEmpty()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerReason.RequiredErrorText)
                .Matches(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerReason.WhiteListHtmlRegularExpression)
                .WithMessage(
                    VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerReason.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousEmployerReason.WhiteListInvalidTagErrorText)
                .When(IsAnonymousEmployer);

            RuleFor(x => x.AnonymousAboutTheEmployer)
                .NotEmpty()
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousAboutTheEmployer.RequiredErrorText)
                .Matches(VacancyOwnerRelationshipViewModelMessages.AnonymousAboutTheEmployer.WhiteListHtmlRegularExpression)
                .WithMessage(
                    VacancyOwnerRelationshipViewModelMessages.AnonymousAboutTheEmployer.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyOwnerRelationshipViewModelMessages.AnonymousAboutTheEmployer.WhiteListInvalidTagErrorText)
                .When(IsAnonymousEmployer);

        }

        private bool IsAnonymousEmployer(VacancyOwnerRelationshipViewModel arg)
        {
            return arg.IsAnonymousEmployer.HasValue && arg.IsAnonymousEmployer.Value;
        }
    }
}