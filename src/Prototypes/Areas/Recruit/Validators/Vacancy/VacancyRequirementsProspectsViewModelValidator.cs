namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class VacancyRequirementsProspectsViewModelClientValidator : AbstractValidator<VacancyRequirementsProspectsViewModel>
    {
        public VacancyRequirementsProspectsViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.DesiredSkills)
                .Matches(VacancyViewModelMessages.DesiredSkills.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListErrorText);

            RuleFor(x => x.FutureProspects)
                .Matches(VacancyViewModelMessages.FutureProspects.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListErrorText);

            RuleFor(x => x.PersonalQualities)
                .Matches(VacancyViewModelMessages.PersonalQualities.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListErrorText);

            RuleFor(x => x.ThingsToConsider)
                .Matches(VacancyViewModelMessages.ThingsToConsider.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListErrorText);

            RuleFor(x => x.DesiredQualifications)
                .Matches(VacancyViewModelMessages.DesiredQualifications.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListErrorText);
        }
    }

    public class VacancyRequirementsProspectsViewModelServerValidator : VacancyRequirementsProspectsViewModelClientValidator
    {
        public VacancyRequirementsProspectsViewModelServerValidator()
        {
            AddServerRules();
        }

        private void AddServerRules()
        {
            RuleFor(x => x.DesiredSkills)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredSkills.RequiredErrorText);

            RuleFor(x => x.FutureProspects)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FutureProspects.RequiredErrorText);

            RuleFor(x => x.PersonalQualities)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PersonalQualities.RequiredErrorText);

            RuleFor(x => x.DesiredQualifications)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.RequiredErrorText);
        }
    }
}