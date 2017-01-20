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
                Over21NationalMinimumWage = 6.70m,
                ValidationErrorMessage = "The wage should not be less than the National Minimum Wage for apprentices"
            },
            //October 1st, 2016
            new WageRange
            {
                ValidFrom = new DateTime(2016, 10, 1),
                ValidTo = new DateTime(2017, 04, 1),
                ApprenticeMinimumWage = 3.40m,
                Under18NationalMinimumWage = 4.00m,
                Between18And20NationalMinimumWage = 5.55m,
                Over21NationalMinimumWage = 6.95m,
                ValidationErrorMessage = "The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Oct 2016"
            },
            //April 1st, 2017
            new WageRange
            {
                ValidFrom = new DateTime(2017, 04, 1),
                ValidTo = DateTime.MaxValue,
                ApprenticeMinimumWage = 3.50m,
                Under18NationalMinimumWage = 4.05m,
                Between18And20NationalMinimumWage = 5.60m,
                Over21NationalMinimumWage = 7.05m,
                ValidationErrorMessage = "The wage should not be less then the new National Minimum Wage for apprentices effective from 1 Apr 2017"
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