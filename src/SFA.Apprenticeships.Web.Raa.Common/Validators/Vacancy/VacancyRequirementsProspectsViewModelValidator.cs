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
                .Matches(VacancyViewModelMessages.DesiredSkills.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListErrorText);

            validator.RuleFor(x => x.FutureProspects)
                .Matches(VacancyViewModelMessages.FutureProspects.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListErrorText);

            validator.RuleFor(x => x.PersonalQualities)
                .Matches(VacancyViewModelMessages.PersonalQualities.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListErrorText);

            validator.RuleFor(x => x.ThingsToConsider)
                .Matches(VacancyViewModelMessages.ThingsToConsider.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListErrorText);

            validator.RuleFor(x => x.DesiredQualifications)
                .Matches(VacancyViewModelMessages.DesiredQualifications.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<VacancyRequirementsProspectsViewModel> validator)
        {
            validator.RuleFor(x => x.DesiredSkills)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredSkills.RequiredErrorText);

            validator.RuleFor(x => x.FutureProspects)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FutureProspects.RequiredErrorText);

            validator.RuleFor(x => x.PersonalQualities)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PersonalQualities.RequiredErrorText);

            validator.RuleFor(x => x.DesiredQualifications)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.RequiredErrorText)
                .When(x => x.VacancyType != VacancyType.Traineeship);
        }
    }
}