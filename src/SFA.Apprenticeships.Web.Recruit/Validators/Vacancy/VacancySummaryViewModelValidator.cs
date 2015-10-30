namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancySummaryViewModelClientValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelClientValidator()
        {
            AddCommonRules();
            RuleSet(RuleSets.Errors, AddCommonRules);
        }

        private void AddCommonRules()
        {
            RuleFor(viewModel => viewModel.WorkingWeek)
                .Length(0, 256)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);

            RuleFor(viewModel => viewModel.LongDescription)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);
        }
    }

    public class VacancySummaryViewModelServerValidator : VacancySummaryViewModelClientValidator
    {
        private const decimal ApprenticeMinimumWage = 3.30m;

        public VacancySummaryViewModelServerValidator()
        {
            AddServerCommonRules();
            RuleSet(RuleSets.Errors, AddServerCommonRules);
            RuleSet(RuleSets.Warnings, AddServerWarningRules);
        }

        private void AddServerCommonRules()
        {
            RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText);

            RuleFor(x => x.HoursPerWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.RequiredErrorText)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeBetween16And40);

            RuleFor(viewModel => (int)viewModel.WageType)
                .InclusiveBetween((int)WageType.ApprenticeshipMinimumWage, (int)WageType.Custom)
                .WithMessage(VacancyViewModelMessages.WageType.RequiredErrorText);

            RuleFor(x => x.Wage)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Wage.RequiredErrorText)
                .When(x => x.WageType == WageType.Custom);

            RuleFor(x => x.Wage)
                .Must(HaveAValidHourRate)
                .When(v => v.WageType == WageType.Custom)
                .When(v => v.WageUnit != WageUnit.NotApplicable)
                .WithMessage(VacancyViewModelMessages.Wage.WageLessThanMinimum);

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText)
                .Must(HaveAValidDuration)
                .WithMessage(VacancyViewModelMessages.Duration.DurationCantBeLessThan12Months);

            RuleFor(x => x.ClosingDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules

            RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules

            RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);
        }

        private void AddServerWarningRules()
        {
            RuleFor(x => x.Duration)
                .Must(VacancySummaryViewModelBusinessRulesExtensions.ExpectedDurationGreaterThanOrEqualToMinimumDuration)
                .WithState(s => ValidationType.Warning);
        }

        private static bool HaveAValidHourRate(VacancySummaryViewModel vacancy, decimal? wage)
        {
            if (!vacancy.Wage.HasValue || !vacancy.HoursPerWeek.HasValue)
                return false;

            var hourRate = GetHourRate(vacancy.Wage.Value, vacancy.WageUnit, vacancy.HoursPerWeek.Value);

            return !(hourRate < ApprenticeMinimumWage);
        }

        private static bool HaveAValidDuration(VacancySummaryViewModel vacancy, int? duration)
        {
            if (!vacancy.HoursPerWeek.HasValue || !vacancy.Duration.HasValue)
                return true;

            if (vacancy.HoursPerWeekBetween30And40() || vacancy.HoursPerWeekGreaterThanOrEqualTo16())
                return vacancy.DurationGreaterThanOrEqualTo12Months();

            return true;
        }

        private static bool HaveAValidHoursPerWeek(decimal? hours)
        {
            return hours.HasValue && hours.Value >= 16 && hours.Value <= 40;
        }

        private static decimal GetHourRate(decimal wage, WageUnit wageUnit, decimal hoursPerWeek)
        {
            switch (wageUnit)
            {
                case WageUnit.Weekly:
                    return wage/hoursPerWeek;
                case WageUnit.Annually:
                    return wage/52m/hoursPerWeek;
                case WageUnit.Monthly:
                    return wage/52m*12/hoursPerWeek;
                case WageUnit.NotApplicable:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, null);
            }
        }
    }
}