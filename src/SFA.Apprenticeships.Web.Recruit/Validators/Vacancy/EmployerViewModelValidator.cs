namespace SFA.Apprenticeships.Web.Recruit.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Vacancy;

    public class EmployerViewModelValidator : AbstractValidator<EmployerViewModel>
    {
        public EmployerViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(EmployerViewModelMessages.Description.RequiredErrorText)
                .Matches(EmployerViewModelMessages.Description.WhiteListRegularExpression)
                .WithMessage(EmployerViewModelMessages.Description.WhiteListErrorText);
        }
    }
}