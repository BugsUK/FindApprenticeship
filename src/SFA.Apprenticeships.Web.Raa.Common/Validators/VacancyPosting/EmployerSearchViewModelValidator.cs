using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.Validators.VacancyPosting
{
    using FluentValidation;
    using Raa.Common.Constants.ViewModels;


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
                .WithMessage(EmployerSearchViewModelMessages.Ern.RequiredErrorText)
                .Matches(EmployerSearchViewModelMessages.Ern.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.Ern.WhiteListErrorText);

            RuleFor(x => x.Name)
                .Matches(EmployerSearchViewModelMessages.Name.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.Name.WhiteListErrorText);

            RuleFor(x => x.Location)
                .Matches(EmployerSearchViewModelMessages.Location.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.Location.WhiteListErrorText);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Location))
                .When(x => x.FilterType == EmployerFilterType.NameAndLocation)
                .WithMessage(EmployerSearchViewModelMessages.NameAndLocationSearchRequiredErrorText);
        }
    }

    public class EmployerSearchViewModelServerValidator : EmployerSearchViewModelClientValidator
    {

    }
}