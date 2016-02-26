namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Common.ViewModels.Applications;
    using FluentValidation;

    public class TraineeshipApplicationViewModelClientValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelClientValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelClientValidator());
        }
    }

    public class TraineeshipApplicationViewModelServerValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelServerValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelServerValidator());
        }
    }
}