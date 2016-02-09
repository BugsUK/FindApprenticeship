namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Provider;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using VacancyPosting;

    public class VacancyViewModel
    {
        public const string PartialView = "Vacancy/VacancyPreview";

        public long VacancyReferenceNumber { get; set; }

        public NewVacancyViewModel NewVacancyViewModel { get; set; }

        public TrainingDetailsViewModel TrainingDetailsViewModel { get; set; }

        public VacancySummaryViewModel VacancySummaryViewModel { get; set; }

        public VacancyRequirementsProspectsViewModel VacancyRequirementsProspectsViewModel { get; set; }

        public VacancyQuestionsViewModel VacancyQuestionsViewModel { get; set; }

        public List<SelectListItem> ApprenticeshipLevels { get; set; }

        public string FrameworkName { get; set; }

        public string StandardName { get; set; }

        public ProviderSiteViewModel ProviderSite { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        [Display(Name = VacancyViewModelMessages.ResubmitOptin.LabelText)]
        public bool ResubmitOption { get; set; }

        public string BasicDetailsLink { get; set; }

        public string TrainingDetailsLink { get; set; }

        public string SummaryLink { get; set; }

        public string RequirementsProspectsLink { get; set; }

        public string QuestionsLink { get; set; }

        public string EmployerLink { get; set; }
        
        public string LocationsLink { get; set; }

        public int ApplicationCount { get; set; }

        public int OfflineApplicationClickThroughCount { get; set; }

        public List<VacancyLocationAddressViewModel> LocationAddresses { get; set; }

        public bool IsUnapprovedMultiLocationParentVacancy
        {
            get
            {
                return
                    Status != ProviderVacancyStatuses.ParentVacancy
                    && NewVacancyViewModel.LocationAddresses != null
                    && NewVacancyViewModel.LocationAddresses.Count() > 1;
            }
        }

        public bool IsApprovedMultiLocationChildVacancy
        {
            get
            {
                return Status == ProviderVacancyStatuses.Live
                       && NewVacancyViewModel.LocationAddresses != null
                       && NewVacancyViewModel.LocationAddresses.Count() == 1;
            }
        }

        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }

        public int NumberOfPositions { get; set; }

        public ContactDetailsAndVacancyHistoryViewModel ContactDetailsAndVacancyHistory { get; set; }

        public bool IsEditable { get; set; }
    }
}