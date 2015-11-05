using System;

namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentValidation.Attributes;
    using Provider;
    using Validators.Vacancy;

    [Validator(typeof(NewVacancyViewModelClientValidator))]
    public class NewVacancyViewModel
    {
        public long? VacancyReferenceNumber { get; set; }

        public string Ukprn { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string FrameworkCodeName { get; set; }

        [Display(Name = VacancyViewModelMessages.Title.LabelText)]
        public string Title { get; set; }

        [Display(Name = VacancyViewModelMessages.ShortDescription.LabelText)]
        public string ShortDescription { get; set; }

        public TrainingType TrainingType { get; set; }

        public List<SelectListItem> SectorsAndFrameworks { get; set; }

        public int? StandardId { get; set; }

        public List<SelectListItem> Standards { get; set; }

        public ProviderSiteEmployerLinkViewModel ProviderSiteEmployerLink { get; set; }

        public bool OfflineVacancy { get; set; }

        [Display(Name = VacancyViewModelMessages.OfflineApplicationUrl.LabelText)]
        public string OfflineApplicationUrl { get; set; }

        [Display(Name = VacancyViewModelMessages.OfflineApplicationInstructions.LabelText)]
        public string OfflineApplicationInstructions { get; set; }

        public Guid VacancyGuid { get; set; }
    }
}
