namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Provider;
    using Validators.Vacancy;

    [Validator(typeof(VacancyViewModelValidator))]
    public class VacancyViewModel
    {
        public long VacancyReferenceNumber { get; set; }
        public string Ukprn { get; set; }

        [Display(Name = VacancyViewModelMessages.Title.LabelText)]
        public string Title { get; set; }
        [Display(Name = VacancyViewModelMessages.ShortDescription.LabelText)]
        public string ShortDescription { get; set; }
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
        public List<SelectListItem> ApprenticeshipLevels { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string FrameworkCodeName { get; set; }
        public string FrameworkName { get; set; }
        [Display(Name = VacancyViewModelMessages.LongDescription.LabelText)]
        public string LongDescription { get; set; }
        [Display(Name = VacancyViewModelMessages.DesiredSkills.LabelText)]
        public string DesiredSkills { get; set; }
        [Display(Name = VacancyViewModelMessages.FutureProspects.LabelText)]
        public string FutureProspects { get; set; }
        [Display(Name = VacancyViewModelMessages.PersonalQualities.LabelText)]
        public string PersonalQualities { get; set; }
        [Display(Name = VacancyViewModelMessages.ThingsToConsider.LabelText)]
        public string ThingsToConsider { get; set; }
        [Display(Name = VacancyViewModelMessages.DesiredQualifications.LabelText)]
        public string DesiredQualifications { get; set; }
        [Display(Name = VacancyViewModelMessages.FirstQuestion.LabelText)]
        public string FirstQuestion { get; set; }
        [Display(Name = VacancyViewModelMessages.SecondQuestion.LabelText)]
        public string SecondQuestion { get; set; }
        public EmployerViewModel Employer { get; set; }
        public ProviderSiteViewModel ProviderSite { get; set; }
    }
}