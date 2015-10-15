namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof(VacancySummaryViewModelClientValidator))]
    public class VacancySummaryViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        [Display(Name = VacancyViewModelMessages.WorkingWeek.LabelText)]
        public string WorkingWeek { get; set; }
        [Display(Name = VacancyViewModelMessages.WeeklyWage.LabelText)]
        public string WeeklyWage { get; set; }
        [Display(Name = VacancyViewModelMessages.Duration.LabelText)]
        public string Duration { get; set; }
        [Display(Name = VacancyViewModelMessages.ClosingDate.LabelText)]
        public DateTime? ClosingDate { get; set; }
        [Display(Name = VacancyViewModelMessages.PossibleStartDate.LabelText)]
        public DateTime? PossibleStartDate { get; set; }
        [Display(Name = VacancyViewModelMessages.LongDescription.LabelText)]
        public string LongDescription { get; set; }
    }
}