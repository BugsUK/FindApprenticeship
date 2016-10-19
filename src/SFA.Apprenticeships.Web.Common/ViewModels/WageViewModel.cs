namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies;

    public class WageViewModel
    {
        public WageViewModel()
        {
            Type = WageType.Custom;
            Unit = WageUnit.NotApplicable;
        }

        public WageViewModel(Wage wage)
        {
            HoursPerWeek = wage.HoursPerWeek;
            Type = wage.Type;
            Amount = wage.Amount;
            AmountLowerBound = wage.AmountLowerBound;
            AmountUpperBound= wage.AmountUpperBound;
            Text = wage.Text;
            Unit = wage.Unit;
        }

        public WageType Type { get; set; }

        [Display(Name = WageViewModelMessages.LabelText)]
        public decimal? Amount { get; set; }

        public decimal? AmountLowerBound { get; set; }

        public decimal? AmountUpperBound { get; set; }

        public string Text { get; set; }

        public WageUnit Unit { get; set; }

        [Display(Name = WageViewModelMessages.HoursPerWeek.LabelText)]
        public decimal? HoursPerWeek { get; set; }
    }
}
