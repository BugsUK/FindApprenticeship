namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyStatus
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Application;

    public class BulkDeclineCandidatesViewModelClientValidator : AbstractValidator<BulkDeclineCandidatesViewModel>
    {
        public BulkDeclineCandidatesViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class BulkDeclineCandidatesViewModelServerValidator : AbstractValidator<BulkDeclineCandidatesViewModel>
    {
        public BulkDeclineCandidatesViewModelServerValidator()
        {
            this.AddCommonRules();

            RuleFor(m => m.SelectedApplicationIds)
                 .NotEmpty()
                .WithMessage(BulkDeclineCandidatesViewModelMessages.NoApplicationIdsErrorText);

            RuleFor(m => m.UnSuccessfulReason)
                .NotEmpty()
                .WithMessage(ApplicationViewModelMessages.UnSuccessfulReason.RequiredErrorText)
                .When(m => m.UnSuccessfulReasonRequired);
        }
    }

    internal static class BulkDeclineCandidatesViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<BulkDeclineCandidatesViewModel> validator)
        {
            validator.RuleFor(m => m.UnSuccessfulReason)
                .Matches(ApplicationViewModelMessages.UnSuccessfulReason.WhiteListRegularExpression)
                .WithMessage(ApplicationViewModelMessages.UnSuccessfulReason.WhiteListErrorText);
        }
    }
}