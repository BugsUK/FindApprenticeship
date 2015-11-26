namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using System;
    using Constants;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            this.AddCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
        }
    }

    public class VacancySummaryViewModelServerValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerValidator() : this(null)
        {
            
        }
        public VacancySummaryViewModelServerValidator(string parentPropertyName)
        {
            this.AddCommonRules();
            this.AddServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerCommonRules);
            RuleSet(RuleSets.Warnings, () => this.AddServerWarningRules(parentPropertyName));
        }
    }

    public class VacancySummaryViewModelServerErrorValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerErrorValidator()
        {
            this.AddCommonRules();
            this.AddServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerCommonRules);
        }
    }

    public class VacancySummaryViewModelServerWarningValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelServerWarningValidator() : this(null)
        {
            
        }
        public VacancySummaryViewModelServerWarningValidator(string parentPropertyName)
        {
            RuleSet(RuleSets.Warnings, () => this.AddServerWarningRules(parentPropertyName));
        }
    }

    internal static class VacancySummaryViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<VacancySummaryViewModel> validator)
        {
            validator.RuleFor(viewModel => viewModel.WorkingWeek)
                .Length(0, 250)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.LongDescription)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.ClosingDateComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.DurationComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.LongDescriptionComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.PossibleStartDateComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.WageComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.WorkingWeekComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);
        }

        internal static void AddServerCommonRules(this AbstractValidator<VacancySummaryViewModel> validator)
        {
            validator.RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText);

            validator.RuleFor(x => x.HoursPerWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.RequiredErrorText);

            validator.RuleFor(x => x.HoursPerWeek)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeGreaterThan16)
                .When(x => x.HoursPerWeek.HasValue);

            validator.RuleFor(viewModel => (int)viewModel.WageType)
                .InclusiveBetween((int)WageType.ApprenticeshipMinimumWage, (int)WageType.Custom)
                .WithMessage(VacancyViewModelMessages.WageType.RequiredErrorText);

            validator.RuleFor(x => x.Wage)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Wage.RequiredErrorText)
                .When(x => x.WageType == WageType.Custom);

            validator.RuleFor(x => x.Wage)
                .Must(HaveAValidHourRate)
                .When(v => v.WageType == WageType.Custom)
                .When(v => v.WageUnit != WageUnit.NotApplicable)
                .When(v => v.HoursPerWeek.HasValue)
                .WithMessage(VacancyViewModelMessages.Wage.WageLessThanMinimum);

            validator.RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText)
                .Must(HaveAValidDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationCantBeLessThan12Months);

            validator.RuleFor(x => x.ClosingDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .Must(Common.BeOneDayInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
                .Must(Common.BeOneDayInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.AfterTodayErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules

            validator.RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText);
        }

        internal static void AddServerWarningRules(this AbstractValidator<VacancySummaryViewModel> validator, string parentPropertyName)
        {
            validator.Custom(x => x.ExpectedDurationGreaterThanOrEqualToMinimumDuration(x.Duration, parentPropertyName));

            validator.RuleFor(x => x.ClosingDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning);

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning)
                .When(x => x.ClosingDate == null || !x.ClosingDate.HasValue);

            validator.Custom(x => x.PossibleStartDateShouldBeAfterClosingDate(x.ClosingDate, parentPropertyName));
        }

        private static bool HaveAValidHourRate(VacancySummaryViewModel vacancy, decimal? wage)
        {
            if (!vacancy.Wage.HasValue || !vacancy.HoursPerWeek.HasValue)
                return false;

            var hourRate = GetHourRate(vacancy.Wage.Value, vacancy.WageUnit, vacancy.HoursPerWeek.Value);

            return !(hourRate < Wages.ApprenticeMinimumWage);
        }

        private static bool HaveAValidDuration(VacancySummaryViewModel vacancy, decimal? duration)
        {
            if (!vacancy.HoursPerWeek.HasValue || !vacancy.Duration.HasValue)
                return true;

            if (duration.HasValue && duration.Value % 1 != 0)
                return false;

            if (vacancy.HoursPerWeekBetween30And40() || vacancy.HoursPerWeekGreaterThanOrEqualTo16())
                return vacancy.DurationGreaterThanOrEqualTo12Months();

            return true;
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