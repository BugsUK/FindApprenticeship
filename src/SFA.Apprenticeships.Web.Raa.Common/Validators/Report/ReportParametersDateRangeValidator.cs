namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Report
{
    using FluentValidation;
    using Constants.ViewModels;
    using ViewModels.Report;
    using Web.Common.Validators;

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
            validator.RuleFor(x => x.FromDate)
                .Must(x => x.HasValue)
                .WithMessage(ReportParametersMessages.FromDateMessages.RequiredErrorText);
            validator.RuleFor(x => x.FromDate)
                .Must(Common.BeValidDate)
                .WithMessage(ReportParametersMessages.FromDateMessages.InvalidDateErrorText)
                .When(x => x.FromDate.HasValue);

            validator.RuleFor(x => x.ToDate)
                .Must(x => x.HasValue)
                .WithMessage(ReportParametersMessages.ToDateMessages.RequiredErrorText);
            validator.RuleFor(x => x.ToDate)
                .Must(Common.BeValidDate)
                .WithMessage(ReportParametersMessages.ToDateMessages.InvalidDateErrorText)
                .When(x => x.ToDate.HasValue);
        }
    }
}