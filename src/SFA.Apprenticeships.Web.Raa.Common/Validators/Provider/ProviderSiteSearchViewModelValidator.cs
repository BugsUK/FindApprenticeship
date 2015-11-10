using FluentValidation;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    public class ProviderSiteSearchViewModelValidator : AbstractValidator<ProviderSiteSearchViewModel>
    {
        public ProviderSiteSearchViewModelValidator()
        {
            AddEmployerReferenceRules();
        }

        private void AddEmployerReferenceRules()
        {
            RuleFor(m => m.EmployerReferenceNumber)
                .NotEmpty()
                .When(m => m.SiteSearchMode == ProviderSiteSearchMode.EmployerReferenceNumber)
                .WithMessage(ProviderSiteSearchViewModelMessages.EmployerReferenceNumberMessages.RequiredErrorText)
                .Matches("^\\s*\\d+\\s*$")
                .When(m => m.SiteSearchMode == ProviderSiteSearchMode.EmployerReferenceNumber)
                .WithMessage(ProviderSiteSearchViewModelMessages.EmployerReferenceNumberMessages.MustBeNumericText);
        }
    }
}