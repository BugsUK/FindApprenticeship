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
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class NewVacancyViewModelServerValidator : AbstractValidator<NewVacancyViewModel>
    {
        public NewVacancyViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }
            
    internal static class NewVacancyViewModelServerValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            validator.RuleFor(m => m.Title)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            validator.RuleFor(x => x.ShortDescription)
                .Length(0, 512)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);

            RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText);

            validator.RuleFor(m => m.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FrameworkCodeName.RequiredErrorText);

            validator.RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
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

                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Higher)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText);

        }

        internal static void AddClientRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            validator.RuleFor(m => m.OfflineApplicationUrl)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            validator.RuleFor(m => m.OfflineApplicationUrl)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText)
                .When(viewModel => viewModel.OfflineVacancy);
        }
    }
}
