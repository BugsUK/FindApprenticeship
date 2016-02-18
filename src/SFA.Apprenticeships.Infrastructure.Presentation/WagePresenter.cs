namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Constants;
    using Domain.Entities.Raa.Vacancies;

    public static class WagePresenter
    {
        public static string GetHeaderDisplayText(this Wage wage)
        {
            if (wage.Type != WageType.Custom) return "Weekly wage";

            return wage.Unit.GetHeaderDisplayText();
        }

        public static string GetHeaderDisplayText(this WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case WageUnit.Annually:
                    return "Annual wage";
                case WageUnit.Monthly:
                    return "Monthly wage";
                case WageUnit.Weekly:
                    return "Weekly wage";
                default:
                    return string.Empty;
            }
        }

        public static string GetHeaderDisplayText(this Domain.Entities.Vacancies.WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case Domain.Entities.Vacancies.WageUnit.Annually:
                    return "Annual wage";
                case Domain.Entities.Vacancies.WageUnit.Monthly:
                    return "Monthly wage";
                case Domain.Entities.Vacancies.WageUnit.Weekly:
                    return "Weekly wage";
                default:
                    return string.Empty;
            }
        }

        public static string GetWagePostfix(this Domain.Entities.Vacancies.WageUnit wageUnit)
        {
            switch (wageUnit)
            {
                case Domain.Entities.Vacancies.WageUnit.Annually:
                    return "p/year";
                case Domain.Entities.Vacancies.WageUnit.Monthly:
                    return "p/month";
                case Domain.Entities.Vacancies.WageUnit.Weekly:
                    return "p/week";
                default:
                    return string.Empty;
            }
        }

        public static string GetDisplayText(this Wage wage, decimal? hoursPerWeek)
        {
            switch (wage.Type)
            {
                case WageType.Custom:
                    return $"£{wage.Amount?.ToString() ?? "unknown"}";
                case WageType.ApprenticeshipMinimumWage:
                    return hoursPerWeek.HasValue ? GetWeeklyApprenticeshipMinimumWage(hoursPerWeek.Value) : "unknown";
                case WageType.NationalMinimumWage:
                    return hoursPerWeek.HasValue ? GetWeeklyNationalMinimumWage(hoursPerWeek.Value) : "unknown";
                default:
                    return string.Empty;
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
            var lowerRange = (Wages.Under18NationalMinimumWage * hoursPerWeek).ToString("N2");
            var higherRange = (Wages.Over21NationalMinimumWage * hoursPerWeek).ToString("N2");

            return $"£{lowerRange} - £{higherRange}";
        }

        private static string GetWeeklyApprenticeshipMinimumWage(decimal hoursPerWeek)
        {
            return $"£{(Wages.ApprenticeMinimumWage * hoursPerWeek).ToString("N2")}";
        }
    }
}