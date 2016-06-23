namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Attributes;
    using Validators.Vacancy;

    [Validator(typeof(TrainingDetailsViewModelClientValidator))]
    public class TrainingDetailsViewModel : IPartialVacancyViewModel
    {
        public const string PartialView = "Vacancy/TrainingDetails";

        public int? VacancyReferenceNumber { get; set; }

        public VacancyStatus Status { get; set; }

        public VacancyType VacancyType { get; set; }

        public bool ComeFromPreview { get; set; }

        public TrainingType TrainingType { get; set; }

        public string FrameworkCodeName { get; set; }

        [Display(Name = VacancyViewModelMessages.FrameworkCodeNameComment.LabelText)]
        public string FrameworkCodeNameComment { get; set; }

        public List<SelectListItem> SectorsAndFrameworks { get; set; }

        public int? StandardId { get; set; }

        [Display(Name = VacancyViewModelMessages.StandardIdComment.LabelText)]
        public string StandardIdComment { get; set; }

        public List<StandardViewModel> Standards { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        [Display(Name = VacancyViewModelMessages.ApprenticeshipLevelComment.LabelText)]
        public string ApprenticeshipLevelComment { get; set; }

        public string SectorCodeName { get; set; }

        [Display(Name = VacancyViewModelMessages.SectorCodeNameComment.LabelText)]
        public string SectorCodeNameComment { get; set; }

        public List<SelectListItem> Sectors { get; set; }

        [Display(Name = VacancyViewModelMessages.TrainingProvidedMessages.LabelText)]
        public string TrainingProvided { get; set; }

        [Display(Name = VacancyViewModelMessages.TrainingProvidedComment.LabelText)]
        public string TrainingProvidedComment { get; set; }

        [Display(Name = VacancyViewModelMessages.ContactNameMessages.LabelText)]
        public string ContactName { get; set; }

        [Display(Name = VacancyViewModelMessages.ContactNumberMessages.LabelText)]
        public string ContactNumber { get; set; }

        [Display(Name = VacancyViewModelMessages.ContactEmailMessages.LabelText)]
        public string ContactEmail { get; set; }

        [Display(Name = VacancyViewModelMessages.ContactDetailsComment.LabelText)]
        public string ContactDetailsComment { get; set; }

        public int AutoSaveTimeoutInSeconds { get; set; }
    }
}