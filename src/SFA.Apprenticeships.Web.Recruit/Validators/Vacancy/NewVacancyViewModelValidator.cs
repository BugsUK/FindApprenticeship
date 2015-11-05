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
                .Length(0, 350)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 256)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.TooLongErrorText)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationInstructions)
                .Matches(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            
        }

        internal static void AddServerRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            validator.RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Title.RequiredErrorText);

            validator.RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ShortDescription.RequiredErrorText);

            validator.RuleFor(m => m.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FrameworkCodeName.RequiredErrorText);

            validator.RuleFor(m => m.StandardId)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.StandardId.RequiredErrorText);

            validator.RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Higher)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText);

            validator.RuleFor(m => m.OfflineApplicationUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ErrorUriText)
                .When(viewModel => viewModel.OfflineVacancy);
        }
    }
}
