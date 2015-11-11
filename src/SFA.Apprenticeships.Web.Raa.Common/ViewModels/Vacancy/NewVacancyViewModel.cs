using System;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
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

        public List<SelectListItem> SectorsAndFrameworks { get; set; }

        public ProviderSiteEmployerLinkViewModel ProviderSiteEmployerLink { get; set; }

        public bool OfflineVacancy { get; set; }

        [Display(Name = VacancyViewModelMessages.OfflineApplicationUrl.LabelText)]
        public string OfflineApplicationUrl { get; set; }

        [Display(Name = VacancyViewModelMessages.OfflineApplicationInstructions.LabelText)]
        public string OfflineApplicationInstructions { get; set; }

        public Guid VacancyGuid { get; set; }

        public TrainingType TrainingType { get; set; }

        public int? StandardId { get; set; }

        public List<StandardViewModel> Standards { get; set; }

        public string Comment { get; set; }
    }
}
