namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Constants;
    using Domain.Entities.Raa.Vacancies;

    public static class WagePresenter
    {
        public const string AnnualWageText = "Annual wage";
        public const string MonthlyWageText = "Monthly wage";
        public const string WeeklyWageText = "Weekly wage";

        private const string UnknownText = "unknown";
        private const string WageAmountFormat = "N2";

        public static string GetHeaderDisplayText(this Wage wage)
        {
            if (wage.Type != WageType.Custom) return WeeklyWageText;

            return wage.Unit.GetHeaderDisplayText();
        }

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
                default:
                    return "<1000>";
            }
        }

        public static string GetHeaderDisplayText(this Domain.Entities.Vacancies.WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case Domain.Entities.Vacancies.WageUnit.Annually:
                    return AnnualWageText;

                case Domain.Entities.Vacancies.WageUnit.Monthly:
                    return MonthlyWageText;

                case Domain.Entities.Vacancies.WageUnit.Weekly:
                    return WeeklyWageText;

                default:
                    return "<1010>";
            }
        }

        public static string GetWagePostfix(this Domain.Entities.Vacancies.WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case Domain.Entities.Vacancies.WageUnit.Annually:
                    return "per year";
                case Domain.Entities.Vacancies.WageUnit.Monthly:
                    return "per month";
                case Domain.Entities.Vacancies.WageUnit.Weekly:
                    return "per week";
                default:
                    return "<1020>";
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
                    return "<1031>";

                default:
                    return UnknownText;
            }
        }

        public static Domain.Entities.Vacancies.WageUnit GetWageUnit(this Wage wage)
        {
            if (wage.Type == WageType.Custom)
            {
                switch (wage.Unit)
                {
                    case WageUnit.Weekly:
                        return Domain.Entities.Vacancies.WageUnit.Weekly;
                    case WageUnit.Monthly:
                        return Domain.Entities.Vacancies.WageUnit.Weekly;
                    case WageUnit.Annually:
                        return Domain.Entities.Vacancies.WageUnit.Weekly;
                }
            }

            return Domain.Entities.Vacancies.WageUnit.Weekly;
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
