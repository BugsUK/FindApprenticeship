namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Common = Validators.Common;

    public class VacancyRequirementsProspectsViewModelClientValidator : AbstractValidator<VacancyRequirementsProspectsViewModel>
    {
        public VacancyRequirementsProspectsViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class VacancyRequirementsProspectsViewModelServerValidator : AbstractValidator<VacancyRequirementsProspectsViewModel>
    {
        public VacancyRequirementsProspectsViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
            RuleSet(RuleSets.Errors, this.AddServerRules);
        }
    }

    internal static class VacancyRequirementsProspectsViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<VacancyRequirementsProspectsViewModel> validator)
        {
            validator.RuleFor(x => x.DesiredSkills)
                .Matches(VacancyViewModelMessages.DesiredSkills.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListInvalidTagErrorText)
                .When( x => Common.IsNotEmpty(x.DesiredSkills));

            validator.RuleFor(x => x.FutureProspects)
                .Matches(VacancyViewModelMessages.FutureProspects.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListInvalidCharacterErrorText)
                .When(x => !string.IsNullOrEmpty(x.FutureProspects)) //Migrated vacancies can contain just the empty string
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListInvalidTagErrorText);

            validator.RuleFor(x => x.PersonalQualities)
                .Matches(VacancyViewModelMessages.PersonalQualities.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListInvalidTagErrorText);

            validator.RuleFor(x => x.ThingsToConsider)
                .Matches(VacancyViewModelMessages.ThingsToConsider.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListInvalidCharacterErrorText)
                .When(x => !string.IsNullOrEmpty(x.ThingsToConsider)) //Migrated vacancies can contain just the empty string
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListInvalidTagErrorText);

            validator.RuleFor(x => x.DesiredQualifications)
                .Matches(VacancyViewModelMessages.DesiredQualifications.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.DesiredQualifications));
        }

        internal static void AddServerRules(this AbstractValidator<VacancyRequirementsProspectsViewModel> validator)
        {
            validator.RuleFor(x => x.DesiredSkills)
                .NotEmpty()
                .When(x => x.VacancySource == VacancySource.Raa)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.RequiredErrorText);

            validator.RuleFor(x => x.FutureProspects)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FutureProspects.RequiredErrorText);

            validator.RuleFor(x => x.PersonalQualities)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PersonalQualities.RequiredErrorText);

            validator.RuleFor(x => x.DesiredQualifications)
                .NotEmpty()
                .When(x => x.VacancyType != VacancyType.Traineeship)
                .When(x => x.VacancySource == VacancySource.Raa)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.RequiredErrorText);
        }
    }
}