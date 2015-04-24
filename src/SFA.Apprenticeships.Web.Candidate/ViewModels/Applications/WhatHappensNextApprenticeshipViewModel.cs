namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

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

        public bool SentEmail { get; set; }

        public string ProviderContactInfo { get; set; }

        public string SuggestedVacanciesSearchUrl { get; set; }

        public string SuggestedVacanciesCategory { get; set; }

        public string SuggestedVacanciesSearchDistance { get; set; }

        public string SuggestedVacanciesSearchLocation { get; set; }

        public IEnumerable<SuggestedVacancyViewModel> SuggestedVacancies { get; set; } 
    }

    public class SuggestedVacancyViewModel
    {
        public int VacancyId { get; set; }

        public string VacancyTitle { get; set; }

        public string Distance { get; set; }
    }
}