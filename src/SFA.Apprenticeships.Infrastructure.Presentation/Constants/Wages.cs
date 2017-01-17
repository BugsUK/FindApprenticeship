namespace SFA.Apprenticeships.Infrastructure.Presentation.Constants
{
    using System;
    using System.Linq;
    using Entities;

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
                ValidTo = new DateTime(2017, 04, 1),
                ApprenticeMinimumWage = 3.40m,
                Under18NationalMinimumWage = 4.00m,
                Between18And20NationalMinimumWage = 5.55m,
                Over21NationalMinimumWage = 6.95m
            },
            //April 1st, 2017
            new WageRange
            {
                ValidFrom = new DateTime(2017, 04, 1),
                ValidTo = DateTime.MaxValue,
                ApprenticeMinimumWage = 3.50m,
                Under18NationalMinimumWage = 4.05m,
                Between18And20NationalMinimumWage = 5.60m,
                Over21NationalMinimumWage = 7.05m
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
    }
}