namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Provider;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Domain.Entities.Raa.Vacancies;
    using VacancyPosting;
    using Web.Common.ViewModels.Locations;

    public class VacancyViewModel
    {
        public const string PartialView = "Vacancy/VacancyPreview";

        public int VacancyReferenceNumber { get; set; }

        public NewVacancyViewModel NewVacancyViewModel { get; set; }

        public TrainingDetailsViewModel TrainingDetailsViewModel { get; set; }

        public FurtherVacancyDetailsViewModel FurtherVacancyDetailsViewModel { get; set; }

        public VacancyRequirementsProspectsViewModel VacancyRequirementsProspectsViewModel { get; set; }

        public VacancyQuestionsViewModel VacancyQuestionsViewModel { get; set; }

        public List<SelectListItem> ApprenticeshipLevels { get; set; }

        public string FrameworkName { get; set; }

        public string StandardName { get; set; }

        public string SectorName { get; set; }

        public ProviderSiteViewModel ProviderSite { get; set; }

        public VacancyStatus Status { get; set; }

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

        public AddressViewModel Address { get; set; }

        public bool IsUnapprovedMultiLocationParentVacancy => NewVacancyViewModel.LocationAddresses != null
                                                              && NewVacancyViewModel.LocationAddresses.Count() > 1;

        public bool IsApprovedMultiLocationChildVacancy => Status == VacancyStatus.Live
                                                           && NewVacancyViewModel.LocationAddresses != null
                                                           && NewVacancyViewModel.LocationAddresses.Count() == 1;

        public bool IsEmployerLocationMainApprenticeshipLocation { get; set; }

        public int NumberOfPositions { get; set; }

        public ContactDetailsAndVacancyHistoryViewModel ContactDetailsAndVacancyHistory { get; set; }

        public bool IsEditable { get; set; }

        public VacancyType VacancyType { get; set; }

        public bool IsSingleLocation
        {
            get
            {
                return NewVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation == true ||
                       (NewVacancyViewModel.LocationAddresses != null &&
                        NewVacancyViewModel.LocationAddresses.Count == 1);
            }
        }

        public bool IsCandidateView { get; set; }
    }
}