namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    //TODO: remove it
    public class VacancyViewModelValidator : AbstractValidator<VacancyViewModel>
    {
        public VacancyViewModelValidator()
        {
            //RuleSet(RuleSets.Errors, AddCommonRules);
            RuleSet(RuleSets.Warnings, AddCommonRules);
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.VacancySummaryViewModel).SetValidator(new VacancySummaryViewModelServerValidator());
            RuleFor(x => x.NewVacancyViewModel).SetValidator(new NewVacancyViewModelServerValidator());
            RuleFor(x => x.VacancyQuestionsViewModel).SetValidator(new VacancyQuestionsViewModelServerValidator());
            RuleFor(x => x.VacancyRequirementsProspectsViewModel).SetValidator(new VacancyRequirementsProspectsViewModelServerValidator());
        }
    }
}