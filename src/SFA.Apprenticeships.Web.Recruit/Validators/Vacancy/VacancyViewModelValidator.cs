namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using FluentValidation;
    using ViewModels.Vacancy;

    //TODO: remove it
    public class VacancyViewModelValidator : AbstractValidator<VacancyViewModel>
    {
        public VacancyViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.VacancySummaryViewModel).SetValidator(new VacancySummaryViewModelServerValidator());
            RuleFor(x => x.NewVacancyViewModel).SetValidator(new NewVacancyViewModelServerValidator());
            RuleFor(x => x.VacancyQuestionsViewModel).SetValidator(new VacancyQuestionsViewModelServerValidator());
            RuleFor(x => x.VacancyRequirementsProspectsViewModel).SetValidator(new VacancyRequirementsProspectsViewModelServerValidator());
        }
        /*private void AddCommonRules()
        {
            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Title.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(VacancyViewModelMessages.Title.TooLongErrorText)
                .Matches(VacancyViewModelMessages.Title.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.Title.WhiteListErrorText);

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ShortDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.ShortDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.ShortDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.ShortDescription.WhiteListErrorText);

            RuleFor(x => x.WorkingWeek)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.WorkingWeek.RequiredErrorText);

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.Duration.RequiredErrorText);

            RuleFor(x => x.ClosingDate)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.ClosingDate.RequiredErrorText)
                .GreaterThan(DateTime.Today)
                .WithMessage(VacancyViewModelMessages.ClosingDate.TooSoonErrorText);

            RuleFor(x => x.PossibleStartDate)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.RequiredErrorText)
                .GreaterThan(DateTime.Today)
                .WithMessage(VacancyViewModelMessages.PossibleStartDate.TooSoonErrorText);

            RuleFor(x => x.LongDescription)
                .NotEmpty()
                .WithMessage(VacancyViewModelMessages.LongDescription.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.LongDescription.TooLongErrorText)
                .Matches(VacancyViewModelMessages.LongDescription.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.LongDescription.WhiteListErrorText);

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

            RuleFor(x => x.FirstQuestion)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.TooLongErrorText)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            RuleFor(x => x.SecondQuestion)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.TooLongErrorText)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListErrorText);
        }*/
    }
}