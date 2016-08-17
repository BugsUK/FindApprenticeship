namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;

    public class Wage
    {
        public Wage(WageType type, decimal? amount, string text, WageUnit unit)
        {
            Type = type;
            Amount = amount;
            Text = text;
            Unit = CorrectWageUnit(type, unit);
        }

        public WageType Type { get; private set; }

        public decimal? Amount { get; private set; }

        public string Text { get; private set; }

        public WageUnit Unit { get; private set; }

        private static WageUnit CorrectWageUnit(WageType type, WageUnit unit)
        {
            if (type == WageType.LegacyWeekly)
            {
                return WageUnit.Weekly;
            }
            if (type == WageType.LegacyText)
            {
                return WageUnit.NotApplicable;
            }

            if (type != WageType.Custom)
            {
                return WageUnit.Weekly;
            }

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
        }
    }
}
