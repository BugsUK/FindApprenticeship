namespace SFA.Apprenticeships.Web.Common.ViewModels.Applications
{
    using Models.Application;
    using Candidate;
    using VacancySearch;

    public class TraineeshipApplicationViewModel : ApplicationViewModelBase<TraineeshipCandidateViewModel>
    {
        public TraineeshipVacancyDetailViewModel VacancyDetail { get; set; }

        public TraineeshipApplicationViewModel(string message, ApplicationViewModelStatus viewModelStatus)
            : base(message, viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel(string message)
            : base(message)
        {
        }

        public TraineeshipApplicationViewModel(ApplicationViewModelStatus viewModelStatus)
            : base(viewModelStatus)
        {
        }

        public TraineeshipApplicationViewModel()
        {
        }
    }
}