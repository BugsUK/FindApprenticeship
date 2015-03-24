namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using Candidate;
    using Common.Models.Application;
    using VacancySearch;

    public class TraineeshipApplicationViewModel : ApplicationViewModelBase
    {
        public TraineeshipCandidateViewModel Candidate { get; set; }

        public TraineeshipVacancyDetailViewModel VacancyDetail { get; set; }

        public TraineeshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel(string message) : base(message)
        {
        }

        public TraineeshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus) : base(viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel()
        {
        }
    }
}