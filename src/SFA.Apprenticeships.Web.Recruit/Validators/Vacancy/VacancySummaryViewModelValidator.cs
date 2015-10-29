namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System;
    using System.Security.Cryptography.X509Certificates;
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
        }

        private void AddCommonRules()
        {

            RuleFor(viewModel => viewModel.WorkingWeek)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);

            /*RuleFor(viewModel => viewModel.Wage)
                .Matches(VacancyViewModelMessages.WeeklyWage.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WeeklyWage.WhiteListErrorText);

            RuleFor(viewModel => viewModel.Duration)
                .Matches(VacancyViewModelMessages.Duration.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Duration.WhiteListErrorText);*/

            RuleFor(viewModel => viewModel.LongDescription)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText); 
        }
    }

    public class VacancySummaryViewModelServerValidator : VacancySummaryViewModelClientValidator
    {
        private const decimal NationalMinumumWage = 3.87m;
        private const decimal ApprenticeMinimumWage = 3.30m;

        public VacancySummaryViewModelServerValidator()
        {
            AddServerCommonRules();
        }

        private void AddServerCommonRules()
        {
            RuleFor(x => x.ClosingDate).SetValidator(new DateViewModelServerValidator());
            RuleFor(x => x.PossibleStartDate).SetValidator(new DateViewModelServerValidator());

            RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText)
                .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);
            /*.Length(0, 100)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.TooLongErrorText)
            .Matches(VacancyViewModelMessages.WorkingWeek.WhiteListRegularExpression)
            .WithMessage(VacancyViewModelMessages.WorkingWeek.WhiteListErrorText);*/

            RuleFor(x => x.HoursPerWeek)
                .InclusiveBetween(16, 40);

            /*RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText)
                .Matches(VacancyViewModelMessages.Duration.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Duration.WhiteListErrorText);*/

            RuleFor(x => x.ClosingDate).SetValidator(new DateViewModelClientValidator());

            RuleFor(x => x.PossibleStartDate).SetValidator(new DateViewModelClientValidator());

            RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);

            RuleFor(x => x.Wage)
                .Must(HaveAValidHourRate)
                .When(v => v.WageType == WageType.Custom)
                .When(v => v.WageUnit != WageUnit.NotApplicable)
                .WithMessage("TODO: wage should not be less than the Apprenticeship Minimum Wage.");
        }

        private static bool HaveAValidHourRate(VacancySummaryViewModel vacancy, decimal? wage)
        {
            if (!vacancy.Wage.HasValue || !vacancy.HoursPerWeek.HasValue)
                return false;

            var hourRate = GetHourRate(vacancy.Wage.Value, vacancy.WageUnit, vacancy.HoursPerWeek.Value);

            return !(hourRate < ApprenticeMinimumWage);
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