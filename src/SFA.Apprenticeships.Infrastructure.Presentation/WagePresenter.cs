namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;
    using Constants;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;

    public static class WagePresenter
    {
        public const string AnnualWageText = "Annual wage";
        public const string MonthlyWageText = "Monthly wage";
        public const string WeeklyWageText = "Weekly wage";

        public const string PerYearText = "per year";
        public const string PerMonthText = "per month";
        public const string PerWeekText = "per week";

        public const string UnknownText = "unknown";

        private const string WageAmountFormat = "N2";

        public static string GetHeaderDisplayText(this WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case WageUnit.Annually:
                    return AnnualWageText;

                case WageUnit.Monthly:
                    return MonthlyWageText;

                case WageUnit.Weekly:
                    return WeeklyWageText;

                case WageUnit.NotApplicable:
                    return string.Empty;

                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), $"Invalid Wage Unit: {wageUnit}");
            }
        }

        public static string GetDisplayAmountWithFrequencyPostfix(this WageUnit wageUnit, string displayAmount)
        {
            if (string.IsNullOrWhiteSpace(displayAmount))
            {
                return wageUnit.GetWagePostfix();
            }

            return $"{displayAmount} {wageUnit.GetWagePostfix()}";
        }

        private static string GetWagePostfix(this WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case WageUnit.Annually:
                    return PerYearText;

                case WageUnit.Monthly:
                    return PerMonthText;

                case WageUnit.Weekly:
                    return PerWeekText;

                // TODO: HOTFIX: should revert this change.
                default:
                    return string.Empty;

                    /*
                    case WageUnit.NotApplicable:
                        return string.Empty;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(wageUnit), $"Invalid Wage Unit: {wageUnit}");
                    */
            }
        }

        public static string GetDisplayAmount(this Wage wage)
        {
            switch (wage.Type)
            {
                case WageType.LegacyWeekly:
                case WageType.Custom:
                    return $"£{wage.Amount?.ToString(WageAmountFormat) ?? UnknownText}";

                case WageType.ApprenticeshipMinimum:
                    return wage.HoursPerWeek.HasValue
                        ? GetWeeklyApprenticeshipMinimumWage(wage.HoursPerWeek.Value)
                        : UnknownText;

                case WageType.NationalMinimum:
                    return wage.HoursPerWeek.HasValue
                        ? GetWeeklyNationalMinimumWage(wage.HoursPerWeek.Value)
                        : UnknownText;

                case WageType.LegacyText:
                    return wage.Text ?? UnknownText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(wage.Type), $"Invalid Wage Type: {wage.Type}");
            }
        }

        private static string GetWeeklyNationalMinimumWage(decimal hoursPerWeek)
        {
            var lowerRange = (Wages.Under18NationalMinimumWage * hoursPerWeek).ToString(WageAmountFormat);
            var higherRange = (Wages.Over21NationalMinimumWage * hoursPerWeek).ToString(WageAmountFormat);

            return $"£{lowerRange} - £{higherRange}";
        }

        private static string GetWeeklyApprenticeshipMinimumWage(decimal hoursPerWeek)
        {
            return $"£{(Wages.ApprenticeMinimumWage * hoursPerWeek).ToString(WageAmountFormat)}";
        }
    }
}
