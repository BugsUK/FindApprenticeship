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

        public static string GetDisplayAmountWithFrequencyPostfix(WageType type, decimal? amount, string text, WageUnit unit, decimal? hoursPerWeek)
        {
            var postfix = unit.GetWagePostfix();

            var displayAmount = GetDisplayAmount(type, amount, text, unit, hoursPerWeek);
            if (string.IsNullOrWhiteSpace(displayAmount))
            {
                return postfix;
            }

            return $"{displayAmount} {postfix}";
        }

        public static string GetDisplayAmount(WageType type, decimal? amount, string text, WageUnit unit, decimal? hoursPerWeek)
        {
            switch (type)
            {
                case WageType.LegacyWeekly:
                case WageType.Custom:
                    return $"£{amount?.ToString(WageAmountFormat) ?? UnknownText}";

                case WageType.ApprenticeshipMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyApprenticeshipMinimumWage(hoursPerWeek.Value)
                        : UnknownText;

                case WageType.NationalMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyNationalMinimumWage(hoursPerWeek.Value)
                        : UnknownText;

                case WageType.LegacyText:
                    
                    //if it's unknown, return standard unknown text
                    var displayText = text ?? UnknownText;

                    //if it's not unknown, then prepend a '£' sign to its decimal value.
                    decimal wageDecimal;

                    //if it's already got a '£' sign, or is text, fail to parse and all is good => return value.
                    if (decimal.TryParse(displayText, out wageDecimal))
                    {
                        displayText = $"£{wageDecimal:N2}";
                    }

                    return displayText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type,
                        $"Invalid Wage Type: {type}");
            }
        }

        #region private convenience methods

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
        
        #endregion

    }
}
