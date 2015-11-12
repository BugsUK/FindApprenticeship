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
            RuleFor(x => x.FirstQuestion)
                .Matches(VacancyViewModelMessages.FirstQuestion.WhiteListRegularExpression)
                .WithMessage(VacancyViewModelMessages.FirstQuestion.WhiteListErrorText);

            RuleFor(x => x.SecondQuestion)
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
            
        }
    }
}