namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

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

    public class VacancyDatesViewModelCommonValidator : AbstractValidator<VacancyDatesViewModel>
    {
        public VacancyDatesViewModelCommonValidator()
        {
            this.AddCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
        }
    }

    public class VacancyDatesViewModelServerCommonValidator : AbstractValidator<VacancyDatesViewModel>
    {
        public VacancyDatesViewModelServerCommonValidator()
        {
            this.AddServerCommonRules();
            RuleSet(RuleSets.Errors, this.AddServerCommonRules);
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
            validator.RuleFor(viewModel => viewModel.ClosingDateComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.PossibleStartDateComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);
        }

        internal static void AddServerCommonRules(this AbstractValidator<VacancyDatesViewModel> validator)
        {
            validator.RuleFor(x => x.ClosingDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .Must(Common.BeTodayOrInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TodayOrInTheFutureErrorText)
                .SetValidator(new DateViewModelClientValidator());

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeValidDate)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
                .Must(Common.BeOneDayInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.AfterTodayErrorText)
                .SetValidator(new DateViewModelClientValidator());
        }

        internal static void AddServerWarningRules(this AbstractValidator<VacancyDatesViewModel> validator,
            string parentPropertyName)
        {
            validator.RuleFor(x => x.ClosingDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning)
                .When(x => !(x.VacancyStatus == VacancyStatus.Live || x.VacancyStatus == VacancyStatus.Closed));

            validator.RuleFor(x => x.PossibleStartDate)
                .Must(Common.BeTwoWeeksInTheFuture)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText)
                .WithState(s => ValidationType.Warning)
                .When(x => x.ClosingDate == null || !x.ClosingDate.HasValue);

            validator.Custom(x => x.PossibleStartDateShouldBeAfterClosingDate(x.ClosingDate, parentPropertyName));
        }
    }
}