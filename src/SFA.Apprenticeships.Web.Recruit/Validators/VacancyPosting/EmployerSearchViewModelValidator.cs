namespace SFA.Apprenticeships.Web.Recruit.Validators.VacancyPosting
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancyPosting;

    public class EmployerSearchViewModelClientValidator : AbstractValidator<EmployerSearchViewModel>
    {
        public EmployerSearchViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.Ern)
                .NotEmpty()
                .When(x => x.FilterType == EmployerFilterType.Ern)
                .WithMessage(EmployerSearchViewModelMessages.Ern.RequiredErrorText);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Location))
                .When(x => x.FilterType == EmployerFilterType.NameAndLocation)
                .WithMessage(EmployerSearchViewModelMessages.NameAndLocationSearchRequiredErrorText);
        }
    }

    public class EmployerSearchViewModelServerValidator : EmployerSearchViewModelClientValidator
    {

    }
}