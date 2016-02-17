namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System.Collections.Generic;
    using Common.ViewModels.MyApplications;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using VacancySearch;

    public class WhatHappensNextApprenticeshipViewModel : ViewModelBase
    {
        public WhatHappensNextApprenticeshipViewModel()
        {
        }

        public WhatHappensNextApprenticeshipViewModel(string message)
            : base(message)
        {
        }

        public string VacancyTitle { get; set; }

        public string VacancyReference { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public ApplicationStatuses Status { get; set; }

        public string ProviderContactInfo { get; set; }

        public ApprenticeshipSearchViewModel SuggestedVacanciesSearchViewModel { get; set; }

        public string SuggestedVacanciesCategory { get; set; }

        public int SuggestedVacanciesSearchDistance { get; set; }

        public string SuggestedVacanciesSearchLocation { get; set; }

        public IList<SuggestedVacancyViewModel> SuggestedVacancies { get; set; }

        public IList<MyApprenticeshipApplicationViewModel> SavedAndDraftApplications { get; set; }
    }

    public class SuggestedVacancyViewModel
    {
        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        public string Distance { get; set; }
    }
}