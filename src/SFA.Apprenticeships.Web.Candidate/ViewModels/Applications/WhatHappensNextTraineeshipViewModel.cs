namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class WhatHappensNextTraineeshipViewModel : ViewModelBase
    {
        public WhatHappensNextTraineeshipViewModel()
        {
        }

        public WhatHappensNextTraineeshipViewModel(string message)
            : base(message)
        {
        }

        public string VacancyTitle { get; set; }

        public string VacancyReference { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public ApplicationStatuses Status { get; set; }

        public string ProviderContactInfo { get; set; }
    }
}