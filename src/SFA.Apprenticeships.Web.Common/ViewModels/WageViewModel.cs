namespace SFA.Apprenticeships.Web.Common.ViewModels
{
    using System;
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
            RangeUnit = wage.Unit;
            WageTypeReason = wage.ReasonForType;

            switch (wage.Type)
            {
                case WageType.Custom:
                case WageType.CustomRange:
                    Type = WageType.Custom;
                    CustomType = wage.Type;
                    PresetText = wage.Type;
                    break;
                case WageType.CompetitiveSalary:
                case WageType.ToBeAgreedUponAppointment:
                case WageType.Unwaged:
                case WageType.LegacyText:
                    Type = WageType.LegacyText;
                    CustomType = WageType.LegacyText;
                    PresetText = wage.Type;
                    break;
                case WageType.LegacyWeekly:
                case WageType.ApprenticeshipMinimum:
                case WageType.NationalMinimum:
                    Type = wage.Type;
                    CustomType = wage.Type;
                    PresetText = wage.Type;
                    break;
                default:
                    throw new InvalidOperationException("unknown wage type");
            }
        }

        public WageType Type { get; set; }

        public WageType CustomType { get; set; }

        public WageType PresetText { get; set; }

        [Display(Name = WageViewModelMessages.LabelText)]
        public decimal? Amount { get; set; }

        public decimal? AmountLowerBound { get; set; }

        public decimal? AmountUpperBound { get; set; }

        public string WageTypeReason { get; set; }

        public string Text { get; set; }

        public WageUnit Unit { get; set; }

        public WageUnit RangeUnit { get; set; }

        [Display(Name = WageViewModelMessages.HoursPerWeek.LabelText)]
        public decimal? HoursPerWeek { get; set; }
    }

    public enum WageClassification
    {
        ApprenticeshipMinimum = 1,
        NationalMinimum = 2,
        Custom = 3,
        LegacyText = 4,
        PresetText = 5
    }

    public enum CustomWageType
    {
        NotApplicable = 0,
        Fixed = 1,
        Ranged = 2
    }

    public enum PresetText
    {
        NotApplicable = 0,
        CompetitiveSalary = 1,
        ToBeAgreedUponAppointment = 2,
        Unwaged = 3
    }
}
