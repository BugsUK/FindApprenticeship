namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Mvc;
    using Common.ViewModels;
    using Constants;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof(VacancySummaryViewModelClientValidator))]
    public class VacancySummaryViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        [Display(Name = VacancyViewModelMessages.WorkingWeek.LabelText)]
        public string WorkingWeek { get; set; }
        [Display(Name = VacancyViewModelMessages.HoursPerWeek.LabelText)]
        public decimal? HoursPerWeek { get; set; }
        //TODO: Probably create dedicated WageViewModel
        public WageType WageType { get; set; }
        [Display(Name = VacancyViewModelMessages.Wage.LabelText)]
        public decimal? Wage { get; set; }
        public WageUnit WageUnit { get; set; }
        public List<SelectListItem> WageUnits { get; set; }
        public DurationType DurationType { get; set; }
        public List<SelectListItem> DurationTypes { get; set; }
        [Display(Name = VacancyViewModelMessages.Duration.LabelText)]
        public decimal? Duration { get; set; }
        [Display(Name = VacancyViewModelMessages.ClosingDate.LabelText)]
        public DateViewModel ClosingDate { get; set; }
        [Display(Name = VacancyViewModelMessages.PossibleStartDate.LabelText)]
        public DateViewModel PossibleStartDate { get; set; }
        [Display(Name = VacancyViewModelMessages.LongDescription.LabelText)]
        public string LongDescription { get; set; }

        public string WageUnitDisplayText
        {
            get
            {
                if (WageType != WageType.Custom) return "Weekly";

                switch (WageUnit)
                {
                    case WageUnit.Annually:
                        return "Annual";
                    case WageUnit.Monthly:
                        return "Monthly";
                    case WageUnit.Weekly:
                        return "Weekly";
                    default:
                        return string.Empty;
                }
            }
        }

        public string DurationTypeDisplayText
        {
            get
            {
                switch (DurationType)
                {
                    case DurationType.Weeks:
                        return "weeks";
                    case DurationType.Months:
                        return "months";
                    case DurationType.Years:
                        return "years";
                    default:
                        return string.Empty;
                }
            }
        }

        public string WageDisplayText
        {
            get
            {
                switch (WageType)
                {
                    case WageType.Custom:
                        return string.Format("£{0}", Wage.HasValue ? Wage.Value.ToString() : "unknown");
                    case WageType.ApprenticeshipMinimumWage:
                        return HoursPerWeek.HasValue ? GetWeeklyApprenticeshipMinimumWage() : "unknown";
                    case WageType.NationalMinimumWage:
                        return HoursPerWeek.HasValue ? GetWeeklyNationalMinimumWage() : "unknown";
                    default:
                        return string.Empty;
                }
            }
        }

        private string GetWeeklyNationalMinimumWage()
        {
            var lowerRange = (Wages.Under18NationalMinimumWage*HoursPerWeek.Value).ToString(CultureInfo.InvariantCulture);
            var higherRange = (Wages.Over21NationalMinimumWage*HoursPerWeek.Value).ToString(CultureInfo.InvariantCulture);

            return string.Format("£{0} - £{1}", lowerRange, higherRange);
        }

        private string GetWeeklyApprenticeshipMinimumWage()
        {
            return string.Format("£{0}", (Wages.ApprenticeMinimumWage*HoursPerWeek.Value).ToString(CultureInfo.InvariantCulture));
        }
    }
}