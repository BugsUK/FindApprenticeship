namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    public class Wage
    {
        public Wage(WageType type, decimal? amount, WageUnit unit)
        {
            Type = type;
            Amount = amount;
            Unit = unit;
        }

        public WageType Type { get; private set; }
        public decimal? Amount { get; private set; }
        public WageUnit Unit { get; private set; }
    }
}