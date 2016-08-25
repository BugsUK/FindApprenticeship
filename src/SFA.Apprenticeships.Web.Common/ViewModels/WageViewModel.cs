namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using Domain.Entities.Vacancies;

    public class WageViewModel
    {
        public WageType Type { get; private set; }
        public decimal? Amount { get; private set; }
        public string Text { get; private set; }
        public WageUnit Unit { get; private set; }
        public decimal? HoursPerWeek { get; private set; }

        public WageViewModel(WageType type, decimal? amount, string text, WageUnit unit, decimal? hoursPerWeek)
        {
            Type = type;
            Amount = amount;
            Text = text;
            Unit = unit;
            HoursPerWeek = hoursPerWeek;
        }

        public WageViewModel(Wage wage)
        {
            HoursPerWeek = wage.HoursPerWeek;
            Type = wage.Type;
            Amount = wage.Amount;
            Text = wage.Text;
            Unit = wage.Unit;
        }
    }
}
