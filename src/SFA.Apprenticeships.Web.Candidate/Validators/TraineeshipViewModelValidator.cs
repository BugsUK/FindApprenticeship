namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using ViewModels.Candidate;

    public class TraineeshipViewModelClientValidator : CandidateViewModelClientValidatorBase<TraineeshipCandidateViewModel>
    {
    }

    public class TraineeshipViewModelServerValidator : CandidateViewModelServerValidatorBase<TraineeshipCandidateViewModel>
    {
        public TraineeshipViewModelServerValidator()
        {
            RuleFor(x => x.MonitoringInformation).SetValidator(new MonitoringInformationViewModelValidator());
        }
    }
}