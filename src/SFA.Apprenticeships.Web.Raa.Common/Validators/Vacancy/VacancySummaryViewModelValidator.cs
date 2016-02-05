namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using System;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using Infrastructure.Presentation.Constants;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
        }
    }

    public class VacancySummaryViewModelServerValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            this.AddVacancySummaryViewModelServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelServerCommonRules);
            RuleSet(RuleSets.Warnings, () => this.AddVacancySummaryViewModelServerWarningRules(null));
        }
    }

    public class VacancySummaryViewModelServerErrorValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerErrorValidator()
        {
            this.AddVacancySummaryViewModelCommonRules();
            this.AddVacancySummaryViewModelServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelCommonRules);
            RuleSet(RuleSets.Errors, this.AddVacancySummaryViewModelServerCommonRules);
        }
    }

    public class VacancySummaryViewModelServerWarningValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerWarningValidator(string parentPropertyName)
        {
            RuleSet(RuleSets.Warnings, () => this.AddVacancySummaryViewModelServerWarningRules(parentPropertyName));
        }
    }

    internal static class VacancySummaryViewModelValidatorRules
    {
        internal static void AddVacancySummaryViewModelCommonRules(this AbstractValidator<VacancySummaryViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.WorkingWeek)
                .Length(0, 250)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(viewModel => viewModel.WorkingWeek)
                .Length(0, 250)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TraineeshipTooLongErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TraineeshipWhiteListErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship);

            validator.RuleFor(viewModel => viewModel.LongDescription)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);

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
        }

        internal static void AddVacancySummaryViewModelServerCommonRules(this AbstractValidator<VacancySummaryViewModel> validator)
        {
            validator.RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TraineeshipRequiredErrorText)
                .When(x => x.VacancyType == VacancyType.Traineeship);

            validator.RuleFor(x => x.HoursPerWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.HoursPerWeek)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeGreaterThan16)
                .When(x => x.HoursPerWeek.HasValue);

            validator.RuleFor(viewModel => (int)viewModel.WageType)
                .InclusiveBetween((int)WageType.ApprenticeshipMinimumWage, (int)WageType.Custom)
                .WithMessage(VacancyViewModelMessages.WageType.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Wage)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Wage.RequiredErrorText)
                .When(x => x.WageType == WageType.Custom)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Wage)
                .Must(HaveAValidHourRate)
                .When(v => v.WageType == WageType.Custom)
                .When(v => v.WageUnit != WageUnit.NotApplicable)
                .When(v => v.HoursPerWeek.HasValue)
                .WithMessage(VacancyViewModelMessages.Wage.WageLessThanMinimum);

            validator.RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText);

            validator.RuleFor(x => x.Duration)
                .Must(HaveAValidApprenticeshipDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationCantBeLessThan12Months)
                .When(x => x.VacancyType != VacancyType.Traineeship);

            validator.RuleFor(x => x.Duration)
                .Must(HaveAValidTraineeshipDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationMustBeBetweenSixWeeksAndSixMonths)
                .When(x => x.VacancyType == VacancyType.Traineeship);
            
            validator.RuleFor(x => x.VacancyDatesViewModel).SetValidator(new VacancyDatesViewModelServerCommonValidator());

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText);
        }

        internal static void AddVacancySummaryViewModelServerWarningRules(this AbstractValidator<VacancySummaryViewModel> validator, string parentPropertyName)
        {
            validator.Custom(x => x.ExpectedDurationGreaterThanOrEqualToMinimumDuration(x.Duration, parentPropertyName));

            var parentPropertyNameToUse = string.IsNullOrWhiteSpace(parentPropertyName)
                ? "VacancyDatesViewModel"
                : parentPropertyName + ".VacancyDatesViewModel";

            validator.RuleFor(x => x.VacancyDatesViewModel)
                .SetValidator(new VacancyDatesViewModelServerWarningValidator(parentPropertyNameToUse));
        }

        private static bool HaveAValidHourRate(VacancySummaryViewModel vacancy, decimal? wage)
        {
            if (vacancy.VacancyType == VacancyType.Traineeship && !vacancy.Wage.HasValue)
                return true;

            if (!vacancy.Wage.HasValue || !vacancy.HoursPerWeek.HasValue)
                return false;

            var hourRate = GetHourRate(vacancy.Wage.Value, vacancy.WageUnit, vacancy.HoursPerWeek.Value);

            return !(hourRate < Wages.ApprenticeMinimumWage);
        }

        private static bool HaveAValidApprenticeshipDuration(VacancySummaryViewModel vacancy, decimal? duration)
        {
            if (!vacancy.HoursPerWeek.HasValue || !vacancy.Duration.HasValue)
                return true;

            if (duration.HasValue && duration.Value % 1 != 0)
                return false;

            if (vacancy.HoursPerWeekBetween30And40() || vacancy.HoursPerWeekGreaterThanOrEqualTo16())
                return vacancy.DurationGreaterThanOrEqualTo12Months();

            return true;
        }

        private static bool HaveAValidTraineeshipDuration(VacancySummaryViewModel vacancy, decimal? duration)
        {
            if (!vacancy.Duration.HasValue)
                return true;

            if (vacancy.DurationType == DurationType.Years)
                return false;

            if (duration.HasValue && duration.Value % 1 != 0)
                return false;

            return vacancy.DurationBetweenSixWeeksAndSixMonths();
        }

        private static bool HaveAValidHoursPerWeek(decimal? hours)
        {
            return hours.HasValue && hours.Value >= 16;
        }

        private static decimal GetHourRate(decimal wage, WageUnit wageUnit, decimal hoursPerWeek)
        {
            switch (wageUnit)
            {
                case WageUnit.Weekly:
                    return wage / hoursPerWeek;
                case WageUnit.Annually:
                    return wage / 52m / hoursPerWeek;
                case WageUnit.Monthly:
                    return wage / 52m * 12 / hoursPerWeek;
                case WageUnit.NotApplicable:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, null);
            }
        }
    }
}