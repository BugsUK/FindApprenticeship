namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;

    public class VacancyQuestionsViewModelClientValidator : AbstractValidator<VacancyQuestionsViewModel>
    {
        public VacancyQuestionsViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class VacancyQuestionsViewModelServerValidator : AbstractValidator<VacancyQuestionsViewModel>
    {
        public VacancyQuestionsViewModelServerValidator()
        {
            this.AddCommonRules();
            RuleSet(RuleSets.Errors, this.AddCommonRules);
        }
    }

    internal static class VacancyQuestionsViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<VacancyQuestionsViewModel> validator)
        {
            validator.RuleFor(x => x.FirstQuestion)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            validator.RuleFor(x => x.SecondQuestion)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListErrorText);

            validator.RuleFor(x => x.FirstQuestionComment)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            validator.RuleFor(x => x.SecondQuestionComment)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListErrorText);
        }
    }
}