namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
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

            validator.RuleFor(viewModel => viewModel.VacancyDatesViewModel)
                .SetValidator(new VacancyDatesViewModelCommonValidator());

            validator.RuleFor(x => x.ExpectedDuration)
                .Matches(VacancyViewModelMessages.ExpectedDuration.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.ExpectedDuration.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.ExpectedDuration.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.ExpectedDuration));
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
                        x.Wage.Type == WageType.ApprenticeshipMinimum || x.Wage.Type == WageType.NationalMinimum);

            validator.RuleFor(x => x.Wage.HoursPerWeek)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeGreaterThan16)
                .When(x => x.Wage.HoursPerWeek.HasValue);

            validator.RuleFor(viewModel => (int)viewModel.Wage.Type)
                .InclusiveBetween((int)WageType.ApprenticeshipMinimum, (int)WageType.Custom)
                .WithMessage(VacancyViewModelMessages.WageType.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Wage.Amount)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Wage.RequiredErrorText)
                .When(x => x.Wage.Type == WageType.Custom)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.Custom(x => x.HaveAValidHourRate(x.Wage.Amount, parentPropertyName));

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
            
            validator.RuleFor(x => x.VacancyDatesViewModel).SetValidator(new VacancyDatesViewModelServerCommonValidator());

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.TraineeshipRequiredErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship);
        }

        internal static void AddVacancySummaryViewModelServerWarningRules(this AbstractValidator<FurtherVacancyDetailsViewModel> validator, string parentPropertyName)
        {
            validator.Custom(x => x.ExpectedDurationGreaterThanOrEqualToMinimumDuration(x.Duration, parentPropertyName));

            var parentPropertyNameToUse = string.IsNullOrWhiteSpace(parentPropertyName)
                ? "VacancyDatesViewModel"
                : parentPropertyName + ".VacancyDatesViewModel";

            validator.RuleFor(x => x.VacancyDatesViewModel)
                .SetValidator(new VacancyDatesViewModelServerWarningValidator(parentPropertyNameToUse));
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