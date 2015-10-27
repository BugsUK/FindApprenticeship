namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class VacancyQuestionsViewModelClientValidator : AbstractValidator<VacancyQuestionsViewModel>
    {
        public VacancyQuestionsViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
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
        }
    }

    public class VacancyQuestionsViewModelServerValidator : VacancyQuestionsViewModelClientValidator
    {
        public VacancyQuestionsViewModelServerValidator()
        {
            AddServerRules();
        }

        private void AddServerRules()
        {
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
        }
    }
}