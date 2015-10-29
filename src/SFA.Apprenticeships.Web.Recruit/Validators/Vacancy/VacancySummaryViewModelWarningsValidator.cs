namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancySummaryViewModelWarningsValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelWarningsValidator()
        {
            RuleFor(x => x.Duration)
                .Must(VacancySummaryViewModelBusinessRulesExtensions.ExpectedDurationGreaterThanOrEqualToMinimumDuration)
                .WithState(s => ValidationType.Warning);
        }
    }
}