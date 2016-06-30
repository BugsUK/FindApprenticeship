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
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidCharacterErrorText)
                .Must(Validators.Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidTagErrorText);

            validator.RuleFor(x => x.SecondQuestion)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListHtmlRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidCharacterErrorText)
                .Must(Validators.Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidTagErrorText);

            validator.RuleFor(x => x.FirstQuestionComment)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidCharacterErrorText);

            validator.RuleFor(x => x.SecondQuestionComment)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidCharacterErrorText)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidCharacterErrorText);
        }
    }
}