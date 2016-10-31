namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Web.Common.ViewModels;
    using Common = Common;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<FurtherVacancyDetailsViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
        }
    }

    public class VacancySummaryViewModelServerValidator : AbstractValidator<FurtherVacancyDetailsViewModel>
    {
        public VacancySummaryViewModelServerValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            this.AddVacancySummaryViewModelServerCommonRules(null);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
            RuleSet(RuleSets.Errors, () => this.AddVacancySummaryViewModelServerCommonRules(null));
            RuleSet(RuleSets.Warnings, () => this.AddVacancySummaryViewModelServerWarningRules(null));
        }
    }

    public class VacancySummaryViewModelDatesServerValidator : AbstractValidator<FurtherVacancyDetailsViewModel>
    {
        public VacancySummaryViewModelDatesServerValidator()
        {
            this.AddVacancySummaryViewModelDatesCommonRules();
            this.AddVacancySummaryViewModelDatesServerCommonRules(null);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelDatesCommonRules);
            RuleSet(RuleSets.Errors, () => this.AddVacancySummaryViewModelDatesServerCommonRules(null));
            RuleSet(RuleSets.Warnings, () => this.AddVacancySummaryViewModelDatesServerWarningRules(null));
        }
    }

    public class VacancySummaryViewModelServerErrorValidator : AbstractValidator<FurtherVacancyDetailsViewModel>
    {
        public VacancySummaryViewModelServerErrorValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            this.AddVacancySummaryViewModelServerCommonRules(null);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
            RuleSet(RuleSets.Errors, () => this.AddVacancySummaryViewModelServerCommonRules(null));
        }

        public VacancySummaryViewModelServerErrorValidator(string parentPropertyName)
        {
            this.AddVacancySummaryViewModelCommonRules();
            this.AddVacancySummaryViewModelServerCommonRules(parentPropertyName);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
            RuleSet(RuleSets.Errors, () => this.AddVacancySummaryViewModelServerCommonRules(parentPropertyName));
        }
    }

    public class VacancySummaryViewModelServerWarningValidator : AbstractValidator<FurtherVacancyDetailsViewModel>
    {
        public VacancySummaryViewModelServerWarningValidator(string parentPropertyName)
        {
            RuleSet(RuleSets.Warnings, () => this.AddVacancySummaryViewModelServerWarningRules(parentPropertyName));
        }
    }    
    
    internal static class VacancySummaryViewModelValidatorRules
    {
        internal static void AddVacancySummaryViewModelDatesCommonRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.VacancyDatesViewModel)
                .SetValidator(new VacancyDatesViewModelCommonValidator());
        }

        internal static void AddVacancySummaryViewModelCommonRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.WorkingWeek)
                .Length(0, 250)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);                

            validator.RuleFor(viewModel => viewModel.LongDescription)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListInvalidTagErrorText);

            validator.RuleFor(viewModel => viewModel.DurationComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.LongDescriptionComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.WageComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.WorkingWeekComment)                
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            AddVacancySummaryViewModelDatesCommonRules(validator);

            validator.RuleFor(x => x.ExpectedDuration)
                .Matches(VacancyViewModelMessages.ExpectedDuration.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.ExpectedDuration.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.ExpectedDuration.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.ExpectedDuration));

            validator.RuleFor(x => x.Wage.CustomType)
                .Must(ct => ct != CustomWageType.NotApplicable)
                .WithMessage(VacancyViewModelMessages.CustomWageType.RequiredErrorText)
                .When(x => x.Wage.Classification == WageClassification.Custom);

            validator.RuleFor(x => x.Wage.AmountLowerBound)
                .Must(ct => ct.HasValue)
                .WithMessage(VacancyViewModelMessages.AmountLower.RequiredErrorText)
                .When(x => x.Wage.Classification == WageClassification.Custom
                    && x.Wage.CustomType == CustomWageType.Ranged);

            validator.RuleFor(x => x.Wage.AmountUpperBound)
                .Must(ct => ct.HasValue)
                .WithMessage(VacancyViewModelMessages.AmountUpper.RequiredErrorText)
                .When(x => x.Wage.Classification == WageClassification.Custom
                    && x.Wage.CustomType == CustomWageType.Ranged);
        }

        internal static void AddVacancySummaryViewModelDatesServerCommonRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator, string parentPropertyName)
        {
            validator.Custom(x => x.HaveAValidHourRate(x.Wage.Amount, parentPropertyName, "Wage.Amount"));

            validator.Custom(x => x.HaveAValidHourRate(x.Wage.AmountLowerBound, parentPropertyName, "Wage.AmountLowerBound"));

            validator.RuleFor(x => x.VacancyDatesViewModel).SetValidator(new VacancyDatesViewModelServerCommonValidator());
        }

        internal static void AddVacancySummaryViewModelServerCommonRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator, string parentPropertyName)
        {
            validator.RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TraineeshipRequiredErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship);

            validator.RuleFor(x => x.Wage.HoursPerWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship)
                .When(
                    x =>
                        x.VacancySource == VacancySource.Raa || x.Duration.HasValue ||
                        x.Wage.Classification == WageClassification.ApprenticeshipMinimum || x.Wage.Classification == WageClassification.NationalMinimum);

            validator.RuleFor(x => x.Wage.HoursPerWeek)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeGreaterThan16)
                .When(x => x.Wage.HoursPerWeek.HasValue);

            validator.RuleFor(viewModel => (int)viewModel.Wage.Classification)
                .InclusiveBetween((int)WageClassification.ApprenticeshipMinimum, (int)WageClassification.PresetText)
                .WithMessage(VacancyViewModelMessages.WageClassification.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Wage.Amount)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Wage.RequiredErrorText)
                .When(x => x.Wage.Classification == WageClassification.Custom)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText)
                .When(x => x.VacancySource == VacancySource.Raa);

            validator.RuleFor(x => x.Duration)
                .Must(HaveAValidApprenticeshipDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationCantBeLessThan12Months)
                .When(x => x.VacancyType != VacancyType.Traineeship)
                .When(x => x.VacancySource == VacancySource.Raa || x.Wage.HoursPerWeek.HasValue);

            validator.RuleFor(x => x.Duration)
                .Must(HaveAValidTraineeshipDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationMustBeBetweenSixWeeksAndSixMonths)
                .When(x => x.VacancyType == VacancyType.Traineeship)
                .When(x => x.VacancySource == VacancySource.Raa);

            AddVacancySummaryViewModelDatesServerCommonRules(validator, parentPropertyName);

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.TraineeshipRequiredErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship);
        }

        internal static void AddVacancySummaryViewModelDatesServerWarningRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator, string parentPropertyName)
        {
            validator.Custom(x => x.ExpectedDurationGreaterThanOrEqualToMinimumDuration(x.Duration, parentPropertyName));

            var parentPropertyNameToUse = string.IsNullOrWhiteSpace(parentPropertyName)
                ? "VacancyDatesViewModel"
                : parentPropertyName + ".VacancyDatesViewModel";

            validator.RuleFor(x => x.VacancyDatesViewModel)
                .SetValidator(new VacancyDatesViewModelServerWarningValidator(parentPropertyNameToUse));
        }

        internal static void AddVacancySummaryViewModelServerWarningRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator, string parentPropertyName)
        {
            AddVacancySummaryViewModelDatesServerWarningRules(validator, parentPropertyName);
        }

        private static bool HaveAValidApprenticeshipDuration(FurtherVacancyDetailsViewModel furtherVacancy, decimal? duration)
        {
            if (!furtherVacancy.Wage.HoursPerWeek.HasValue || !furtherVacancy.Duration.HasValue)
                return true;

            if (duration.HasValue && duration.Value % 1 != 0)
                return false;

            if (furtherVacancy.HoursPerWeekBetween30And40() || furtherVacancy.HoursPerWeekGreaterThanOrEqualTo16())
                return furtherVacancy.DurationGreaterThanOrEqualTo12Months();

            return true;
        }

        private static bool HaveAValidTraineeshipDuration(FurtherVacancyDetailsViewModel furtherVacancy, decimal? duration)
        {
            if (!furtherVacancy.Duration.HasValue)
                return true;

            if (furtherVacancy.DurationType == DurationType.Years)
                return false;

            if (duration.HasValue && duration.Value % 1 != 0)
                return false;

            return furtherVacancy.DurationBetweenSixWeeksAndSixMonths();
        }

        private static bool HaveAValidHoursPerWeek(decimal? hours)
        {
            return hours.HasValue && hours.Value >= 16;
        }
    }
}