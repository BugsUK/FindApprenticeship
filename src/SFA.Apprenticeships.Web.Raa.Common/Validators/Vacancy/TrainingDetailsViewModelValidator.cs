namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentValidation;
    using Constants.ViewModels;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class TrainingDetailsViewModelClientValidator : AbstractValidator<TrainingDetailsViewModel>
    {
        public TrainingDetailsViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class TrainingDetailsViewModelServerValidator : AbstractValidator<TrainingDetailsViewModel>
    {
        public TrainingDetailsViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerRules);
        }
    }

    internal static class TrainingDetailsViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<TrainingDetailsViewModel> validator)
        {
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

        internal static void AddClientRules(this AbstractValidator<TrainingDetailsViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<TrainingDetailsViewModel> validator)
        {
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
        }
    }
}