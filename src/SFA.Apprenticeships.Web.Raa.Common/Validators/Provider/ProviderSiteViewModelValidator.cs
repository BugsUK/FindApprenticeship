namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Provider
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Provider;
    using Web.Common.Validators;
    using Common = Common;

    public class ProviderSiteViewModelClientValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class ProviderSiteViewModelServerValidator : AbstractValidator<ProviderSiteViewModel>
    {
        public ProviderSiteViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class ProviderSiteViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {
            validator.RuleFor(m => m.EdsUrn)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.RequiredErrorText)
                .Length(9, 9)
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.RequiredLengthErrorText)
                .Matches(ProviderSiteViewModelMessages.EdsUrn.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.EdsUrn.WhiteListErrorText);

            validator.RuleFor(m => m.FullName)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.FullName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.FullName.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.FullName.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.FullName.WhiteListErrorText);

            validator.RuleFor(m => m.TradingName)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.TradingName.RequiredErrorText)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.TradingName.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.TradingName.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.TradingName.WhiteListErrorText);

            validator.RuleFor(m => m.EmployerDescription)
                .Matches(ProviderSiteViewModelMessages.EmployerDescription.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.EmployerDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(ProviderSiteViewModelMessages.EmployerDescription.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.EmployerDescription));

            validator.RuleFor(m => m.CandidateDescription)
                .Matches(ProviderSiteViewModelMessages.CandidateDescription.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.CandidateDescription.WhiteListInvalidCharacterErrorText)
                .Must(Common.BeAValidFreeText)
                .WithMessage(ProviderSiteViewModelMessages.CandidateDescription.WhiteListInvalidTagErrorText)
                .When(x => Common.IsNotEmpty(x.CandidateDescription));

            validator.RuleFor(m => m.ContactDetailsForEmployer)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.ContactDetailsForEmployer.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.ContactDetailsForEmployer.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.ContactDetailsForEmployer.WhiteListErrorText);

            validator.RuleFor(m => m.ContactDetailsForCandidate)
                .Length(0, 255)
                .WithMessage(ProviderSiteViewModelMessages.ContactDetailsForCandidate.TooLongErrorText)
                .Matches(ProviderSiteViewModelMessages.ContactDetailsForCandidate.WhiteListRegularExpression)
                .WithMessage(ProviderSiteViewModelMessages.ContactDetailsForCandidate.WhiteListErrorText);

            validator.RuleFor(x => x.Address).SetValidator(new AddressViewModelValidator());

            validator.RuleFor(m => m.WebPage)
                .NotEmpty()
                .WithMessage(ProviderSiteViewModelMessages.WebPage.RequiredErrorText)
                .Length(0, 100)
                .WithMessage(ProviderSiteViewModelMessages.WebPage.TooLongErrorText)
                .Must(Common.IsValidUrl)
                .WithMessage(ProviderSiteViewModelMessages.WebPage.ErrorUriText);
        }

        internal static void AddClientRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<ProviderSiteViewModel> validator)
        {

        }
    }
}