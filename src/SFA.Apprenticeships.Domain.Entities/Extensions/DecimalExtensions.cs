namespace SFA.Apprenticeships.Domain.Entities.Extensions
{
    public static class DecimalExtensions
    {
        private const int WeeksInAYear = 52;
        private const int MonthsInAYear = 12;

        public static decimal YearsToWeeks(this decimal value)
        {
            return value*WeeksInAYear;
        }

        public static decimal MonthsToWeeks(this decimal value)
        {
            return value* WeeksInAYear/MonthsInAYear;
        }
    }
}
