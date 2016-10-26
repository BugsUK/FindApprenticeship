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
            Classification = WageClassification.Custom;
            CustomType = CustomWageType.NotApplicable;
            PresetText = PresetText.NotApplicable;
        }

        public WageType Type { get; set; }

        public WageClassification Classification { get; set; }

        public CustomWageType CustomType { get; set; }

        public PresetText PresetText { get; set; }

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
        LegacyText = 0,
        ApprenticeshipMinimum = 1,
        NationalMinimum = 2,
        Custom = 3,
        PresetText = 4
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
