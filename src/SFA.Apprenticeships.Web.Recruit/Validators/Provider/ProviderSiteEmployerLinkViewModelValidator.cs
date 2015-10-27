namespace SFA.Apprenticeships.Web.Recruit.Validators.Provider
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
                .Must(uri =>
                {
                    if (string.IsNullOrEmpty(uri)) { return true; }
                    if (!uri.StartsWith("http://") && !uri.StartsWith("https://")) { uri = $"http://{uri}"; }
                    try
                    {
                        Uri outUri;
                        if (Uri.TryCreate(uri, UriKind.Absolute, out outUri)
                            && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
                        {
                            return true;
                        }
                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(ProviderSiteEmployerLinkViewModelMessages.WebsiteUrl.ErrorUriText);
        }
    }
}