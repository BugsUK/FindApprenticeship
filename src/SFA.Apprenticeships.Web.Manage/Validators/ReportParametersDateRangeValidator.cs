namespace SFA.Apprenticeships.Web.Manage.Validators
{
    using Common.Validators;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels;

    public class ReportParametersDateRangeValidator : AbstractValidator<ReportParameterBase>
    {
        public ReportParametersDateRangeValidator()
        {
            this.AddCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
        }
    }

    internal static class ReportParametersDateRangeValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ReportParameterBase> validator)
        {
            validator.RuleFor(x => x.ToDate)
                .Must(x => x.HasValue)
                .WithMessage(ReportViewModelMessages.ToDateRequired);

            validator.RuleFor(x => x.FromDate)
                .Must(x => x.HasValue)
                .WithMessage(ReportViewModelMessages.FromDateRequired);
        }
    }
}