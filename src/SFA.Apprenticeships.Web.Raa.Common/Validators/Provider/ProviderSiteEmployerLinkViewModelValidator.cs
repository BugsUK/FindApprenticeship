namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using System;
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

            RuleFor(x => x.WebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.WebsiteUrl.ErrorUriText)
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));

            RuleFor(x => x.IsEmployerLocationMainApprenticeshipLocation)
                .NotNull()
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.IsEmployerLocationMainApprenticeshipLocation.RequiredErrorText);

            RuleFor(x => x.NumberOfPositions)
                .NotEmpty()
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.NumberOfPositions.RequiredErrorText)
                .InclusiveBetween(1, 999)
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.NumberOfPositions.LengthErrorText)
                .When(x => x.IsEmployerLocationMainApprenticeshipLocation.HasValue && x.IsEmployerLocationMainApprenticeshipLocation == true);
        }
    }
}