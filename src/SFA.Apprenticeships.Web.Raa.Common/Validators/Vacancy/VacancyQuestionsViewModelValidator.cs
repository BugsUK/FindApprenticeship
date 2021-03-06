﻿namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Common = Validators.Common;

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
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.TooLongErrorText)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidCharacterErrorText)
                .Must(Validators.Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.FirstQuestion));

            validator.RuleFor(x => x.SecondQuestion)
                .Length(0, 4000)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.TooLongErrorText)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidCharacterErrorText)
                .Must(Validators.Common.BeAValidFreeText)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.SecondQuestion));

            validator.RuleFor(x => x.FirstQuestionComment)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListInvalidCharacterErrorText);

            validator.RuleFor(x => x.SecondQuestionComment)
                .Matches(VacancyViewModelMessages.SecondQuestion.WhiteListTextRegularExpression)
                .WithMessage(VacancyViewModelMessages.SecondQuestion.WhiteListInvalidCharacterErrorText);
        }
    }
}