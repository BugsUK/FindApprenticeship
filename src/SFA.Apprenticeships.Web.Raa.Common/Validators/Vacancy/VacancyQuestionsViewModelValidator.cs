namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
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
            //TODO: why we are adding only these rules to client side validation. What happens if user has Javascript deactivated?
            RuleFor(x => x.FirstQuestion)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            RuleFor(x => x.SecondQuestion)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListErrorText);

            RuleFor(x => x.FirstQuestionComment)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            RuleFor(x => x.SecondQuestionComment)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListErrorText);
        }
    }

    public class VacancyQuestionsViewModelServerValidator : VacancyQuestionsViewModelClientValidator
    {
    }
}