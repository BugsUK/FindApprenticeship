namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Common.Validators;
    using Common.ViewModels.Candidate;

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