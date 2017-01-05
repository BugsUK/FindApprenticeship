namespace SFA.Apprenticeships.Web.Recruit.Validators
{
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies.Validation;
    using FluentValidation;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.VacancyManagement;

    public class EditWageViewModelValidator : AbstractValidator<EditWageViewModel>
    {
        public EditWageViewModelValidator()
        {
            Custom(WageUpdateValidator.WageTypeValidation);

            Custom(WageUpdateValidator.WageAmountRequiredValidation);
            Custom(WageUpdateValidator.WageAmountLowerBoundRequiredValidation);
            Custom(WageUpdateValidator.WageAmountUpperBoundRequiredValidation);
            Custom(WageUpdateValidator.WageUnitRequiredValidation);
            Custom(WageUpdateValidator.WageRangeBasicValidation);

            Custom(WageUpdateValidator.WageAmountValidation);

            RuleFor(viewModel => (int)viewModel.Classification)
                .InclusiveBetween((int)WageClassification.ApprenticeshipMinimum, (int)WageClassification.PresetText)
                .WithMessage(VacancyViewModelMessages.WageClassification.RequiredErrorText);

            RuleFor(x => x.CustomType)
                .Must(ct => ct != CustomWageType.NotApplicable)
                .WithMessage(VacancyViewModelMessages.CustomWageType.RequiredErrorText)
                .When(x => x.Classification == WageClassification.Custom);
        }
    }
}