namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Application
{
    using FluentValidation;
    using ViewModels.Application;

    public class BulkApplicationsRejectViewModelClientValidator : AbstractValidator<BulkDeclineCandidatesViewModel>
    {
        public BulkApplicationsRejectViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class BulkApplicationsRejectViewModelServerValidator : AbstractValidator<BulkDeclineCandidatesViewModel>
    {
        public BulkApplicationsRejectViewModelServerValidator()
        {
            this.AddCommonRules();

            RuleFor(m => m.ApplicationIds)
                .NotNull()
                .WithMessage(BulkApplicationsRejectViewModelMessages.NoApplicationIdsErrorText);
        }
    }

    internal static class BulkApplicationsRejectViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<BulkDeclineCandidatesViewModel> validator)
        {

        }
    }
}