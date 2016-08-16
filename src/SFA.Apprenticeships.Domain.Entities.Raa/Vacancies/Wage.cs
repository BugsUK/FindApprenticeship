namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using Entities.Vacancies;

    public class Wage
    {
        public Wage(WageType type, decimal? amount, string text, WageUnit unit)
        {
            Type = type;
            Amount = amount;
            Text = text;
            Unit = unit;
        }

        public WageType Type { get; private set; }

        public decimal? Amount { get; private set; }

        public string Text { get; private set; }

        public WageUnit Unit { get; private set; }
    }
}
