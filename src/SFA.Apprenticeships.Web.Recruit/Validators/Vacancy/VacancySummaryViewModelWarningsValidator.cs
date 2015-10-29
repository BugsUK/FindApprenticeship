namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class VacancySummaryViewModelWarningsValidator : AbstractValidator<VacancySummaryViewModel>
    {
        public VacancySummaryViewModelWarningsValidator()
        {
            RuleFor(x => x.Duration)
                .Must(VacancySummaryViewModelBusinessRulesExtensions.ExpectedDurationGreaterThanOrEqualToMinimumDuration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText);
        }
    }
}