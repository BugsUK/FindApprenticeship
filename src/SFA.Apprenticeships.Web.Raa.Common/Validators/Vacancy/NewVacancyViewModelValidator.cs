﻿namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentValidation;
    using Constants.ViewModels;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Common = Validators.Common;

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

            validator.RuleFor(x => x.OfflineVacancy)
                .NotNull()
                .WithMessage(VacancyViewModelMessages.OfflineVacancy.RequiredErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 256)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.TooLongErrorText)
                .Matches(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationUrl)
                .Length(0, 0)
                .When(viewModel => !viewModel.OfflineVacancy.HasValue || viewModel.OfflineVacancy.Value == false)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ShouldBeEmpty);

            validator.RuleFor(m => m.OfflineApplicationUrlComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationInstructions)
                .Matches(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationInstructions.WhiteListErrorText);

            validator.RuleFor(viewModel => viewModel.OfflineApplicationInstructions)
                .Length(0, 0)
                .When(viewModel => !viewModel.OfflineVacancy.HasValue || viewModel.OfflineVacancy.Value == false)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationInstructions.ShouldBeEmptyText);

            validator.RuleFor(m => m.OfflineApplicationInstructionsComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(m => m.StandardIdComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(m => m.ApprenticeshipLevelComment)
                .Matches(VacancyViewModelMessages.Comment.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Comment.WhiteListErrorText);

            validator.RuleFor(m => m.FrameworkCodeNameComment)
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

            validator.RuleFor(viewModel => (int)viewModel.TrainingType)
                .InclusiveBetween((int)TrainingType.Frameworks, (int)TrainingType.Standards)
                .WithMessage(NewVacancyViewModelMessages.TrainingType.RequiredErrorText);

            validator.RuleFor(m => m.FrameworkCodeName)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FrameworkCodeName.RequiredErrorText)
                .When(m => m.TrainingType == TrainingType.Frameworks);

            validator.RuleFor(m => m.StandardId)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.StandardId.RequiredErrorText)
                .When(m => m.TrainingType == TrainingType.Standards);

            validator.RuleFor(viewModel => (int)viewModel.ApprenticeshipLevel)
                .InclusiveBetween((int)ApprenticeshipLevel.Intermediate, (int)ApprenticeshipLevel.Degree)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText)
                .When(m => m.TrainingType == TrainingType.Frameworks)
                .NotEqual((int)ApprenticeshipLevel.FoundationDegree)
                .WithMessage(NewVacancyViewModelMessages.ApprenticeshipLevel.RequiredErrorText)
                .When(m => m.TrainingType == TrainingType.Frameworks);

            validator.RuleFor(m => m.OfflineApplicationUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(VacancyViewModelMessages.OfflineApplicationUrl.ErrorUriText)
                .When(viewModel => viewModel.OfflineVacancy.HasValue && viewModel.OfflineVacancy.Value == true);
        }
    }
}
