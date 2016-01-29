namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;

    public class TrainingDetailsViewModel
    {
        public const string PartialView = "Vacancy/TrainingDetails";

        public long? VacancyReferenceNumber { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

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
    }
}