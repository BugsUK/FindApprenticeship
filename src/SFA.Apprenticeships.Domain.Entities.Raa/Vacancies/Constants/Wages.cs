namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.Constants
{
    using System;
    using System.Linq;
    using Vacancies;
    using Entities.Vacancies;

    public static class Wages
    {
        public static WageRange[] Ranges = {
            new WageRange
            {
                ValidFrom = DateTime.MinValue,
                ValidTo = new DateTime(2016, 10, 1),
                ApprenticeMinimumWage = 3.30m,
                Under18NationalMinimumWage = 3.87m,
                Between18And20NationalMinimumWage = 5.30m,
                Over21NationalMinimumWage = 6.70m
            },
            //October 1st, 2016
            new WageRange
            {
                ValidFrom = new DateTime(2016, 10, 1),
                ValidTo = DateTime.MaxValue,
                ApprenticeMinimumWage = 3.40m,
                Under18NationalMinimumWage = 4.00m,
                Between18And20NationalMinimumWage = 5.55m,
                Over21NationalMinimumWage = 6.95m
            }
        };

        public static WageRange GetWageRangeFor(this DateTime? dateTime)
        {
            var wageRange = Ranges.Last();
            if (dateTime.HasValue)
            {
                wageRange = Ranges.Single(r => dateTime >= r.ValidFrom && dateTime < r.ValidTo);
            }
            return wageRange;
        }

        public static decimal GetHourRate(decimal wage, WageUnit wageUnit, decimal hoursPerWeek)
        {
            switch (wageUnit)
            {
                case WageUnit.Weekly:
                    return wage / hoursPerWeek;
                case WageUnit.Annually:
                    return wage / 52m / hoursPerWeek;
                case WageUnit.Monthly:
                    return wage / 52m * 12 / hoursPerWeek;
                case WageUnit.NotApplicable:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wageUnit), wageUnit, null);
            }
        }
    }
}