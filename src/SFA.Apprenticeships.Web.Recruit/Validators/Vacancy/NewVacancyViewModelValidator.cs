namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class NewVacancyViewModelClientValidator : AbstractValidator<NewVacancyViewModel>
    {
        public NewVacancyViewModelClientValidator()
        {
            AddCommonRules();
        }

        protected void AddCommonRules()
        {
            RuleFor(viewModel => viewModel.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(NewVacancyViewModelMessages.FrameworkCodeName.RequiredErrorText);

            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Title.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ShortDescription.RequiredErrorText)
                .Length(0, 512)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);
        }
    }

    public class NewVacancyViewModelServerValidator : NewVacancyViewModelClientValidator
    {
        public NewVacancyViewModelServerValidator()
        {
            AddServerRules();
        }

        private void AddServerRules()
        {
            RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Higher)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText);
        }
    }
}
