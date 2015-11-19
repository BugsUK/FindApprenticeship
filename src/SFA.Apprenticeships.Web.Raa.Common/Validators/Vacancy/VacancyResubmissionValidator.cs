namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancyResubmissionValidator : AbstractValidator<VacancyViewModel>
    {
        public VacancyResubmissionValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.ResubmitOptin)
                .Must(x => x)
                .WithMessage(VacancyViewModelMessages.ResubmitOptin.RequiredErrorText);
        }
    }
}