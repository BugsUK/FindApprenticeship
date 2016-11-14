namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Employer
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Employer;

    public class EmployerSearchViewModelClientValidator : AbstractValidator<EmployerSearchViewModel>
    {
        public EmployerSearchViewModelClientValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.EdsUrn)
                .NotEmpty()
                .When(x => x.FilterType == EmployerFilterType.EdsUrn)
                .WithMessage(EmployerSearchViewModelMessages.EdsUrn.RequiredErrorText)
                .Matches(EmployerSearchViewModelMessages.EdsUrn.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.EdsUrn.WhiteListErrorText);

            RuleFor(x => x.Name)
                .Matches(EmployerSearchViewModelMessages.Name.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.Name.WhiteListErrorText);

            RuleFor(x => x.Location)
                .Matches(EmployerSearchViewModelMessages.Location.WhiteListRegularExpression)
                .WithMessage(EmployerSearchViewModelMessages.Location.WhiteListErrorText);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Location))
                .When(x => x.FilterType == EmployerFilterType.NameAndLocation)
                .WithMessage(EmployerSearchViewModelMessages.NameAndLocationSearchRequiredErrorText);

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Location))
                .When(x => x.FilterType == EmployerFilterType.NameOrLocation)
                .WithMessage(EmployerSearchViewModelMessages.NameOrLocationSearchRequiredErrorText);
        }
    }

    public class EmployerSearchViewModelServerValidator : EmployerSearchViewModelClientValidator
    {

    }
}