namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;

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