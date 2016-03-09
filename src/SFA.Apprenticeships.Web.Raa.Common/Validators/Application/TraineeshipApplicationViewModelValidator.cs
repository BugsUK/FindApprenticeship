namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Application
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Application.Traineeship;

    public class TraineeshipApplicationViewModelClientValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class TraineeshipApplicationViewModelServerValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class TraineeshipApplicationViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<TraineeshipApplicationViewModel> validator)
        {
            validator.RuleFor(x => x.Notes)
                .Matches(ApplicationViewModelMessages.Notes.WhiteListRegularExpression)
                .WithMessage(ApplicationViewModelMessages.Notes.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<TraineeshipApplicationViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<TraineeshipApplicationViewModel> validator)
        {

        }
    }
}