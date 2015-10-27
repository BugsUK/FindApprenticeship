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
            
            RuleFor(m => m.Title)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            RuleFor(x => x.ShortDescription)
                .Length(0, 512)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);

            RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText);
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
            RuleFor(viewModel => viewModel.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(NewVacancyViewModelMessages.FrameworkCodeName.RequiredErrorText);

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ShortDescription.RequiredErrorText)
                .Length(0, 512)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText); ;

            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Title.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Higher)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText);

            RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.RequiredErrorText)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText)
                .When(viewModel => viewModel.OfflineVacancy);
        }
    }
}
