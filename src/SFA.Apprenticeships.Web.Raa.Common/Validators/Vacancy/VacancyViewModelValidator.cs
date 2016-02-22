namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancyViewModelValidator : AbstractValidator<VacancyViewModel>
    {
        public VacancyViewModelValidator()
        {
            AddCommonErrorRules();
            RuleSet(RuleSets.Errors, AddCommonErrorRules);
            RuleSet(RuleSets.Warnings, AddCommonWarningRules);
            RuleSet(RuleSets.Resubmission, AddCommonResubmissionRules);
        }

        private void AddCommonErrorRules()
        {
            RuleFor(x => x.NewVacancyViewModel).SetValidator(new NewVacancyViewModelServerValidator());
            RuleFor(x => x.TrainingDetailsViewModel).SetValidator(new TrainingDetailsViewModelServerValidator());
            RuleFor(x => x.FurtherVacancyDetailsViewModel).SetValidator(new VacancySummaryViewModelServerErrorValidator());
            RuleFor(x => x.VacancyRequirementsProspectsViewModel).SetValidator(new VacancyRequirementsProspectsViewModelServerValidator());
            RuleFor(x => x.VacancyQuestionsViewModel).SetValidator(new VacancyQuestionsViewModelServerValidator());
        }

        private void AddCommonWarningRules()
        {
            RuleFor(x => x.FurtherVacancyDetailsViewModel).SetValidator(new VacancySummaryViewModelServerWarningValidator("VacancySummaryViewModel"));
        }

        private void AddCommonResubmissionRules()
        {
            RuleFor(x => x.ResubmitOption)
                .Equal(true)
                .WithMessage(VacancyViewModelMessages.ResubmitOptin.RequiredErrorText)
                .When(x => x.Status == VacancyStatus.RejectedByQA);
        }
    }
}