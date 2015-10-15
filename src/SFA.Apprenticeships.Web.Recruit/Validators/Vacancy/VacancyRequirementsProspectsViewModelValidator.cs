namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using System;
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
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredSkills.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.TooLongErrorText)
                .Matches(VacancyViewModelMessages.DesiredSkills.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredSkills.WhiteListErrorText);

            RuleFor(x => x.FutureProspects)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.FutureProspects.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.FutureProspects.TooLongErrorText)
                .Matches(VacancyViewModelMessages.FutureProspects.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FutureProspects.WhiteListErrorText);

            RuleFor(x => x.PersonalQualities)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PersonalQualities.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.TooLongErrorText)
                .Matches(VacancyViewModelMessages.PersonalQualities.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.PersonalQualities.WhiteListErrorText);

            RuleFor(x => x.ThingsToConsider)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ThingsToConsider.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ThingsToConsider.WhiteListErrorText);

            RuleFor(x => x.DesiredQualifications)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.TooLongErrorText)
                .Matches(VacancyViewModelMessages.DesiredQualifications.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.DesiredQualifications.WhiteListErrorText);
        }
    }

    public class VacancyRequirementsProspectsViewModelServerValidator : VacancyRequirementsProspectsViewModelClientValidator
    {
        
    }
}