namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using FluentValidation;
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Common = Common;
    using VacancyType = Domain.Entities.Vacancies.VacancyType;

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
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerRules);
        }
    }

    internal static class NewVacancyViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<NewVacancyViewModel> validator)
        {
            validator.RuleFor(m => m.Title)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            validator.RuleFor(m => m.TitleComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(x => x.ShortDescription)
                .Length(0, 350)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);

            validator.RuleFor(m => m.ShortDescriptionComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 256)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.TooLongErrorText);
            
            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 0)
                .When(viewModel => !viewModel.OfflineVacancy.HasValue || viewModel.OfflineVacancy.Value == false)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ShouldBeEmpty)
                .When(viewModel => viewModel.VacancySource == VacancySource.Raa);
            
            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText)
                .When(viewModel => viewModel.OfflineVacancy.HasValue && viewModel.OfflineVacancy.Value);

            validator.RuleFor(m => m.OfflineApplicationUrlComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationInstructions)
                .Length(0, 0)
                .When(viewModel => !viewModel.OfflineVacancy.HasValue || viewModel.OfflineVacancy.Value == false)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationInstructions.ShouldBeEmptyText)
                .When(viewModel => viewModel.VacancySource == VacancySource.Raa);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationInstructions)
                .Matches(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListErrorText)
                .When(viewModel => viewModel.OfflineVacancy.HasValue && viewModel.OfflineVacancy.Value)
                .When(viewModel => Common.IsNotEmpty(viewModel.OfflineApplicationInstructions));

            validator.RuleFor(m => m.OfflineApplicationInstructionsComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);
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

            validator.RuleFor(x => x.OfflineVacancy)
                .NotNull()
                .WithMessage(VacancyViewModelMessages.OfflineVacancy.RequiredErrorText);

            validator.RuleFor(m => m.OfflineApplicationUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ErrorUriText)
                .When(viewModel => viewModel.OfflineVacancy.HasValue && viewModel.OfflineVacancy.Value);

            validator.RuleFor(viewModel => (int)viewModel.VacancyType)
                .InclusiveBetween((int)VacancyType.Apprenticeship, (int)VacancyType.Traineeship)
                .WithMessage(VacancyViewModelMessages.VacancyType.RequiredErrorText);
        }
    }
}
