namespace SFA.Apprenticeships.Web.Raa.Common.Validators.VacancyStatus
{
    using FluentValidation;
    using ViewModels.Application.Apprenticeship;

    public class BulkApplicationsRejectViewModelClientValidator : AbstractValidator<BulkApplicationsRejectViewModel>
    {
        public BulkApplicationsRejectViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class BulkApplicationsRejectViewModelServerValidator : AbstractValidator<BulkApplicationsRejectViewModel>
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
        internal static void AddCommonRules(this AbstractValidator<BulkApplicationsRejectViewModel> validator)
        {

        }
    }
}