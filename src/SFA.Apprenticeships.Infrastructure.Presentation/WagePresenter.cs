namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using System;
    using Constants;
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
        
        public static string GetWagePostfix(this Domain.Entities.Vacancies.WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case Domain.Entities.Vacancies.WageUnit.Annually:
                    return PerYearText;

                case Domain.Entities.Vacancies.WageUnit.Monthly:
                    return PerMonthText;

                case Domain.Entities.Vacancies.WageUnit.Weekly:
                    return PerWeekText;

                // TODO: HOTFIX: should revert this change.
                default:
                    return string.Empty;

                    /*
                    case Domain.Entities.Vacancies.WageUnit.NotApplicable:
                        return string.Empty;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(wageUnit), $"Invalid Wage Unit: {wageUnit}");
                    */
            }
        }

        public static string GetDisplayText(this Wage wage, decimal? hoursPerWeek)
        {
            switch (wage.Type)
            {
                case WageType.LegacyWeekly:
                case WageType.Custom:
                    return $"£{wage.Amount?.ToString(WageAmountFormat) ?? UnknownText}";

                case WageType.ApprenticeshipMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyApprenticeshipMinimumWage(hoursPerWeek.Value)
                        : UnknownText;

                case WageType.NationalMinimum:
                    return hoursPerWeek.HasValue
                        ? GetWeeklyNationalMinimumWage(hoursPerWeek.Value)
                        : UnknownText;

                case WageType.LegacyText:
                    return wage.Text ?? UnknownText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(wage.Type), $"Invalid Wage Type: {wage.Type}");
            }
        }

        public static Domain.Entities.Vacancies.WageUnit GetWageUnit(this Wage wage)
        {
            if (wage.Type == WageType.LegacyWeekly)
            {
                return Domain.Entities.Vacancies.WageUnit.Weekly;
            }
            if (wage.Type == WageType.LegacyText)
            {
                return Domain.Entities.Vacancies.WageUnit.NotApplicable;
            }

            if (wage.Type != WageType.Custom)
            {
                return Domain.Entities.Vacancies.WageUnit.Weekly;
            }

            switch (wage.Unit)
            {
                case WageUnit.Weekly:
                    return Domain.Entities.Vacancies.WageUnit.Weekly;

                case WageUnit.Monthly:
                    return Domain.Entities.Vacancies.WageUnit.Monthly;

                case WageUnit.Annually:
                    return Domain.Entities.Vacancies.WageUnit.Annually;

                case WageUnit.NotApplicable:
                    return Domain.Entities.Vacancies.WageUnit.NotApplicable;

                default:
                    throw new ArgumentOutOfRangeException(nameof(wage.Unit), $"Invalid Wage Unit: {wage.Unit}");
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
