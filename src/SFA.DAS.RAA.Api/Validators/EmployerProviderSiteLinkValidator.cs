namespace SFA.DAS.RAA.Api.Validators
{
    using Apprenticeships.Web.Common.Validators;
    using Constants;
    using FluentValidation;
    using Models;

    public class EmployerProviderSiteLinkValidator : AbstractValidator<EmployerProviderSiteLink>
    {
        public EmployerProviderSiteLinkValidator()
        {
            RuleFor(el => el.EmployerEdsUrn)
                .NotEmpty()
                .When(el => !el.EmployerId.HasValue)
                .WithMessage(EmployerProviderSiteLinkMessages.MissingEmployerIdentifier);

            RuleFor(el => el.ProviderSiteId)
                .NotEmpty()
                .When(el => !el.ProviderSiteEdsUrn.HasValue)
                .WithMessage(EmployerProviderSiteLinkMessages.MissingProviderSiteIdentifier);

            RuleFor(el => el.ProviderSiteEdsUrn)
                .NotEmpty()
                .When(el => !el.ProviderSiteId.HasValue)
                .WithMessage(EmployerProviderSiteLinkMessages.MissingProviderSiteIdentifier);

            RuleFor(x => x.EmployerDescription)
                .NotEmpty()
                .WithMessage(EmployerProviderSiteLinkMessages.EmployerDescription.RequiredErrorText)
                .Matches(EmployerProviderSiteLinkMessages.EmployerDescription.WhiteListHtmlRegularExpression)
                .WithMessage(EmployerProviderSiteLinkMessages.EmployerDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(EmployerProviderSiteLinkMessages.EmployerDescription.WhiteListInvalidTagErrorText);

            RuleFor(x => x.EmployerWebsiteUrl)
                .Must(Common.IsValidUrl)
                .WithMessage(EmployerProviderSiteLinkMessages.EmployerWebsiteUrl.InvalidUrlText)
                .When(x => !string.IsNullOrEmpty(x.EmployerWebsiteUrl));
        }
    }
}