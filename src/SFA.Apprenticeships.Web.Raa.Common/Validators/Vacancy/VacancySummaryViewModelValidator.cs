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
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.RequiredErrorText);

            RuleFor(x => x.HoursPerWeek)
                .Must(HaveAValidHoursPerWeek)
                .WithMessage(VacancyViewModelMessages.HoursPerWeek.HoursPerWeekShouldBeGreaterThan16)
                .When(x => x.HoursPerWeek.HasValue);

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
                .When(v => v.HoursPerWeek.HasValue)
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
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
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
            Custom(x => x.ExpectedDurationGreaterThanOrEqualToMinimumDuration(x.Duration));

            RuleFor(x => x.ClosingDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning);

            RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning)
                .When(x => x.ClosingDate == null || !x.ClosingDate.HasValue);

            Custom(x => x.PossibleStartDateShouldBeAfterClosingDate(x.ClosingDate));
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

            if(duration.HasValue && duration.Value % 1 != 0)
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