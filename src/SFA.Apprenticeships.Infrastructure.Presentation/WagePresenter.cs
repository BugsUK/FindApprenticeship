namespace SFA.Apprenticeships.Infrastructure.Presentation
{
    using Constants;
    using Domain.Entities.Vacancies.ProviderVacancies;

    public static class WagePresenter
    {
        public static string GetHeaderDisplayText(this Wage wage)
        {
            if (wage.Type != WageType.Custom) return "Weekly wage";

            switch (wage.Unit)
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