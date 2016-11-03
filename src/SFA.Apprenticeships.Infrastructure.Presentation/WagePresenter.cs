namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;
    using Constants;
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
        public const string CompetitiveSalaryText = "Competitive salary";
        public const string UnwagedText = "Unwaged";
        public const string ToBeAGreedUponAppointmentText = "To be agreed upon appointment";
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

        public static string GetDisplayAmountWithFrequencyPostfix(WageType type, decimal? amount, decimal? amountLowerBound, decimal? amountUpperBound, string text, WageUnit unit, decimal? hoursPerWeek, DateTime? possibleDateTime)
        {
            var postfix = unit.GetWagePostfix();

            var displayAmount = GetDisplayAmount(type, amount, amountLowerBound, amountUpperBound, text, hoursPerWeek, possibleDateTime);
            if (string.IsNullOrWhiteSpace(displayAmount))
            {
                return postfix;
            }

            return $"{displayAmount} {postfix}";
        }

        public static string GetDisplayAmount(WageType type, decimal? amount, decimal? amountLowerBound, decimal? amountUpperBound, string text, decimal? hoursPerWeek, DateTime? possibleDateTime)
        {
            switch (type)
            {
                case WageType.LegacyWeekly:
                case WageType.Custom:
                    return $"£{amount?.ToString(WageAmountFormat) ?? UnknownText}";

                case WageType.ApprenticeshipMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyApprenticeshipMinimumWage(hoursPerWeek.Value, possibleDateTime)
                        : UnknownText;

                case WageType.NationalMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyNationalMinimumWage(hoursPerWeek.Value, possibleDateTime)
                        : UnknownText;

                case WageType.LegacyText:
                    
                    //if it's unknown, return standard unknown text
                    var displayText = text ?? UnknownText;

                    //if it's not unknown, then prepend a '£' sign to its decimal value.
                    decimal wageDecimal;

                    //if it's already got a '£' sign, or is text, fail to parse and all is good => return value.
                    if (decimal.TryParse(displayText, out wageDecimal))
                    {
                        displayText = $"£{wageDecimal.ToString(WageAmountFormat)}";
                    }

                    return displayText;

                case WageType.CustomRange:
                    return $"£{amountLowerBound?.ToString(WageAmountFormat) ?? UnknownText} - £{amountUpperBound?.ToString(WageAmountFormat) ?? UnknownText}";

                case WageType.CompetitiveSalary:
                    return CompetitiveSalaryText;

                case WageType.ToBeAgreedUponAppointment:
                    return ToBeAGreedUponAppointmentText;

                case WageType.Unwaged:
                    return UnwagedText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type,
                        $"Invalid Wage Type: {type}");
            }
        }

        private static string GetWeeklyNationalMinimumWage(decimal hoursPerWeek, DateTime? possibleStartDate)
        {
            var wageRange = possibleStartDate.GetWageRangeFor();

            var lowerRange = (wageRange.Under18NationalMinimumWage * hoursPerWeek).ToString(WageAmountFormat);
            var higherRange = (wageRange.Over21NationalMinimumWage * hoursPerWeek).ToString(WageAmountFormat);

            return $"£{lowerRange} - £{higherRange}";
        }

        private static string GetWeeklyApprenticeshipMinimumWage(decimal hoursPerWeek, DateTime? possibleStartDate)
        {
            var wageRange = possibleStartDate.GetWageRangeFor();

            return $"£{(wageRange.ApprenticeMinimumWage * hoursPerWeek).ToString(WageAmountFormat)}";
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

                case WageUnit.NotApplicable:
                    return string.Empty;

                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), $"Invalid Wage Unit: {wageUnit}");

            }
        }
        
    }
}
