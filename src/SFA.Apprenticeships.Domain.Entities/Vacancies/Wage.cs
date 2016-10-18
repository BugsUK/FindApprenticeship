namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;
    using Newtonsoft.Json;

    public class Wage
    {
        [JsonConstructor]
        public Wage(WageType type, decimal? amount, decimal? lowerBound, decimal? upperBound, string text, WageUnit unit, decimal? hoursPerWeek)
        {
            Type = type;
            Amount = amount;
            AmountLowerBound = lowerBound;
            AmountUpperBound = upperBound;
            Text = text;
            HoursPerWeek = hoursPerWeek;
            Unit = CorrectWageUnit(type, unit);
        }

        public WageType Type { get; private set; }

        public decimal? Amount { get; private set; }

        public decimal? AmountLowerBound { get; private set; }

        public decimal? AmountUpperBound { get; private set; }

        public string Text { get; private set; }

        public WageUnit Unit { get; private set; }

        public decimal? HoursPerWeek { get; private set; }

        private static WageUnit CorrectWageUnit(WageType type, WageUnit unit)
        {
            switch (type)
            {
                case WageType.CustomRange:
                    if (unit == WageUnit.NotApplicable)
                        return WageUnit.Weekly;
                    return unit;

                case WageType.LegacyText:
                case WageType.CompetitiveSalary:
                case WageType.ToBeAgreedUponAppointment:
                case WageType.Unwaged:
                    return WageUnit.NotApplicable;

                case WageType.Custom:
                    switch (unit)
                    {
                        case WageUnit.Weekly:
                        case WageUnit.Monthly:
                        case WageUnit.Annually:
                        case WageUnit.NotApplicable:
                            return unit;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(Unit), $"Invalid Wage Unit: {unit}");
                    }

                case WageType.LegacyWeekly:
                default:
                    return WageUnit.Weekly;
            }
        }
    }
}
