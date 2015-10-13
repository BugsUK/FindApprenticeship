namespace SFA.Apprenticeships.Web.Recruit.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;

    public class ProviderSiteEmployerLinkViewModelValidator : AbstractValidator<ProviderSiteEmployerLinkViewModel>
    {
        public ProviderSiteEmployerLinkViewModelValidator()
        {
            AddCommonRules();
        }

        private void AddCommonRules()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.Description.RequiredErrorText)
                .Matches(ProviderSiteEmployerLinkViewModelMessages.Description.WhiteListRegularExpression)
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.Description.WhiteListErrorText);
        }
    }
}