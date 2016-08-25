namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentValidation.Attributes;
    using Infrastructure.Presentation;
    using Validators.Vacancy;
    using Web.Common.ViewModels;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

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
        
        public WageViewModel WageObject { get; set; }

        public List<SelectListItem> WageUnits { get; set; }

        [Display(Name = VacancyViewModelMessages.WageComment.LabelText)]
        public string WageComment { get; set; }

        public DurationType DurationType { get; set; }

        public List<SelectListItem> DurationTypes { get; set; }

        [Display(Name = VacancyViewModelMessages.Duration.LabelText)]
        public decimal? Duration { get; set; }

        [Display(Name = VacancyViewModelMessages.DurationComment.LabelText)]
        public string DurationComment { get; set; }

        [Display(Name = VacancyViewModelMessages.LegacyExpectedDuration.LabelText)]
        public string ExpectedDuration { get; set; }

        [AllowHtml]
        [Display(Name = VacancyViewModelMessages.LongDescription.LabelText)]
        public string LongDescription { get; set; }

        [Display(Name = VacancyViewModelMessages.LongDescriptionComment.LabelText)]
        public string LongDescriptionComment { get; set; }

        public int WarningsHash { get; set; }

        public bool AcceptWarnings { get; set; }

        public VacancyStatus Status { get; set; }

        public string DurationTypeDisplayText 
        {
            get
            {
                if (!Duration.HasValue)
                {
                    return string.IsNullOrWhiteSpace(ExpectedDuration) ? "Not specified" : ExpectedDuration;
                }

                return new Duration(DurationType, (int?) Duration).GetDisplayText();
            }
        }

        public string WageDisplayText => WagePresenter.GetDisplayAmount(WageObject.Type, WageObject.Amount, null, WageObject.Unit, HoursPerWeek);

        public bool ComeFromPreview { get; set; }

        public VacancyDatesViewModel VacancyDatesViewModel { get; set; }

        public VacancyType VacancyType { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }

        public VacancySource VacancySource { get; set; }
    }
}