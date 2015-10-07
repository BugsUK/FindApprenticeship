namespace SFA.Apprenticeships.Web.Recruit.Validators.VacancyPosting
{
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class NewVacancyViewModelServerValidator : AbstractValidator<NewVacancyViewModel>
    {
        public NewVacancyViewModelServerValidator()
        {
            AddCommonRules();
        }

        #region Helpers

        private void AddCommonRules()
        {
            RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Higher)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText);

            RuleFor(viewModel => viewModel.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(NewVacancyViewModelMessages.FrameworkCodeName.RequiredErrorText);
        }

        #endregion
    }
}
