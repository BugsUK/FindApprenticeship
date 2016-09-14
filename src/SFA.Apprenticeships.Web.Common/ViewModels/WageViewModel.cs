namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies;

    public class WageViewModel
    {
        public WageViewModel()
        : this(WageType.Custom, null, null, WageUnit.NotApplicable, null){ }

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

        public WageType Type { get; set; }

        [Display(Name = WageViewModelMessages.LabelText)]
        public decimal? Amount { get; set; }

        public string Text { get; set; }

        public WageUnit Unit { get; set; }

        [Display(Name = WageViewModelMessages.HoursPerWeek.LabelText)]
        public decimal? HoursPerWeek { get; set; }
    }
}
