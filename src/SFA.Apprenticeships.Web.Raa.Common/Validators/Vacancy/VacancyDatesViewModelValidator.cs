namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancyDatesViewModelClientValidator : AbstractValidator<VacancyDatesViewModel>
    {
        public VacancyDatesViewModelClientValidator()
        {
            this.AddCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
        }
    }

    public class VacancyDatesViewModelServerValidator : AbstractValidator<VacancyDatesViewModel>
    {
        public VacancyDatesViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerCommonRules);
            RuleSet(RuleSets.Warnings, () => this.AddServerWarningRules(null));
        }
    }


    public class VacancyDatesViewModelServerWarningValidator : AbstractValidator<VacancyDatesViewModel>
    {
        public VacancyDatesViewModelServerWarningValidator(string parentPropertyName)
        {
            RuleSet(RuleSets.Warnings, () => this.AddServerWarningRules(parentPropertyName));
        }
    }

    internal static class VacancyDatesViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<VacancyDatesViewModel> validator)
        {

        }

        internal static void AddServerCommonRules(this AbstractValidator<VacancyDatesViewModel> validator)
        {
            validator.RuleFor(x => x.ClosingDate)
                .Must(Web.Common.Validators.Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .Must(Web.Common.Validators.Common.BeOneDayInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.AfterTodayErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Web.Common.Validators.Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
                .Must(Web.Common.Validators.Common.BeOneDayInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.AfterTodayErrorText)
                .SetValidator(new DateViewModelClientValidator()); //Client validatior contains complete rules
        }

        internal static void AddServerWarningRules(this AbstractValidator<VacancyDatesViewModel> validator, string parentPropertyName)
        {
            validator.RuleFor(x => x.ClosingDate)
                .Must(Web.Common.Validators.Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning);

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Web.Common.Validators.Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning)
                .When(x => x.ClosingDate == null || !x.ClosingDate.HasValue);

            validator.Custom(x => x.PossibleStartDateShouldBeAfterClosingDate(x.ClosingDate, parentPropertyName));
        }
    }
}