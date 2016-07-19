namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Infrastructure.Presentation;
    using Validators.Vacancy;

    [Validator(typeof(VacancySummaryViewModelClientValidator))]
    public class FurtherVacancyDetailsViewModel : IPartialVacancyViewModel
    {
        public const string PartialView = "Vacancy/FurtherVacancyDetails";

        public int VacancyReferenceNumber { get; set; }

        [Display(Name = VacancyViewModelMessages.WorkingWeek.LabelText)]
        public string WorkingWeek { get; set; }

        [Display(Name = VacancyViewModelMessages.WorkingWeekComment.LabelText)]
        public string WorkingWeekComment { get; set; }

        [Display(Name = VacancyViewModelMessages.HoursPerWeek.LabelText)]
        public decimal? HoursPerWeek { get; set; }
        
        //TODO: Probably create dedicated WageViewModel
        public WageType WageType { get; set; }

        [Display(Name = VacancyViewModelMessages.Wage.LabelText)]
        public decimal? Wage { get; set; }

        public string WageText { get; set; }

        public WageUnit WageUnit { get; set; }

        public List<SelectListItem> WageUnits { get; set; }

        [Display(Name = VacancyViewModelMessages.WageComment.LabelText)]
        public string WageComment { get; set; }

        public DurationType DurationType { get; set; }

        public List<SelectListItem> DurationTypes { get; set; }

        [Display(Name = VacancyViewModelMessages.Duration.LabelText)]
        public decimal? Duration { get; set; }

        [Display(Name = VacancyViewModelMessages.DurationComment.LabelText)]
        public string DurationComment { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.LongDescription.LabelText)]
        public string LongDescription { get; set; }

        [Display(Name = VacancyViewModelMessages.LongDescriptionComment.LabelText)]
        public string LongDescriptionComment { get; set; }

        public int WarningsHash { get; set; }

        public bool AcceptWarnings { get; set; }

        public VacancyStatus Status { get; set; }

        public string WageUnitDisplayText => new Wage(WageType, Wage, WageText, WageUnit).GetHeaderDisplayText();

        public string DurationTypeDisplayText => new Duration(DurationType, (int?)Duration).GetDisplayText();

        public string WageDisplayText => new Wage(WageType, Wage, WageText, WageUnit).GetDisplayText(HoursPerWeek);

        public bool ComeFromPreview { get; set; }

        public VacancyDatesViewModel VacancyDatesViewModel { get; set; }

        public VacancyType VacancyType { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }

        public VacancySource VacancySource { get; set; }
    }
}