namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Applications;

    public class ApprenticeshipApplicationPreviewViewModelValidator : AbstractValidator<ApprenticeshipApplicationPreviewViewModel>
    {
        public ApprenticeshipApplicationPreviewViewModelValidator()
        {
            RuleFor(x => x.AcceptSubmit)
                .Equal(true)
                .WithMessage(ApprenticeshipApplicationPreviewViewModelMessages.AcceptSubmit.MustAcceptSubmit);
        }
    }
}